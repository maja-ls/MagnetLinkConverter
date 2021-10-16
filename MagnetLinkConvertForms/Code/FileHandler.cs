using MagnetLinkConverter.Utility;
using MonoTorrent;
using MonoTorrent.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace MagnetLinkConverter.Code
{
    public class FileHandler
    {
        private FileSystemWatcher Watcher { get; }
        private ClientEngine ClientEngine { get; }
        private HashSet<string> FilePaths { get; set; }
        private NotificationHelper NotificationHelper { get; }


        public FileHandlerStatus _status;
        public FileHandlerStatus Status { 
            get { 
                return _status;
            }
            private set { 
                _status = value;
                NotificationHelper.UpdateFileHandlerStatus(_status);
            } 
        }

        public FileHandler(NotificationHelper notificationHelper)
        {
            FilePaths = new HashSet<string>();
            Watcher = new FileSystemWatcher();
            ClientEngine = new ClientEngine(new EngineSettings());

            Watcher.Path = SettingsHelper.Values.MagnetPath;
            Watcher.IncludeSubdirectories = true;
            Watcher.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastWrite;
            Watcher.Filter = "*.magnet";
            Watcher.Changed += new FileSystemEventHandler(OnFilesChanged);
            Watcher.Created += new FileSystemEventHandler(OnFilesChanged);


            NotificationHelper = notificationHelper;

            Status = FileHandlerStatus.WatcherStopped;
        }

        public bool WatcherIsRunning()
        {
            return Watcher.EnableRaisingEvents;
        }

        private bool HandlerIsBusy()
        {
            return Status == FileHandlerStatus.ProcessingQueue || Status == FileHandlerStatus.HandlingFiles;
        }

        private void SetFileHandlerStatusFromWatcherStatus()
        {
            if (Watcher.EnableRaisingEvents)
                Status = FileHandlerStatus.IdleWatching;
            else
                Status = FileHandlerStatus.WatcherStopped;
        }

        public void StartWatcher()
        {
            if (SettingsHelper.Values.AllSettingsOk())
            {
                Watcher.EnableRaisingEvents = true;
                if (!HandlerIsBusy())
                    Status = FileHandlerStatus.IdleWatching;
            }
        }
        public void StopWatcher()
        {
            Watcher.EnableRaisingEvents = false;
            if (!HandlerIsBusy())
                Status = FileHandlerStatus.WatcherStopped;
        }

        public void UpdateWatcher()
        {
            StopWatcher();
            Watcher.Path = SettingsHelper.Values.MagnetPath;
            StartWatcher();
        }

        private async void OnFilesChanged(object sender, FileSystemEventArgs e)
        {
            await HandleAllMagnetFilesInDirectory();
        }

        private async Task HandleAllMagnetFilesInDirectory()
        {
            if (!HandlerIsBusy())
            {
                Status = FileHandlerStatus.HandlingFiles;


                var magnetfiles = Directory.GetFiles(SettingsHelper.Values.MagnetPath, "*.magnet", SearchOption.AllDirectories);

                foreach (var f in magnetfiles)
                {
                    if (!FilePaths.Contains(f))
                    {
                        FilePaths.Add(f);
                    }
                }


                SetFileHandlerStatusFromWatcherStatus();

                await ProcessFileQueue();
            }
            else
            {
                Wait(123);
                await HandleAllMagnetFilesInDirectory();
            }
        }

        private async Task ProcessFileQueue()
        {
            if (!HandlerIsBusy())
            {
                Status = FileHandlerStatus.ProcessingQueue;


                var pathList = FilePaths.ToList();
                if (pathList.Count > 0)
                {

                    foreach (var f in pathList)
                    {
                        // DO TORRENT SHIT HERE!!!
                        MagnetLink ml = GetMagnetLink(f);
                        TorrentManager manager = new TorrentManager(ml, SettingsHelper.Values.TorrentFilePath, new TorrentSettings(), SettingsHelper.Values.TorrentFilePath);
                        await ClientEngine.Register(manager);
                        await manager.StartAsync();
                        manager.TorrentStateChanged += TorrentStateChanged;
                        int attempts = 0;
                        while (!manager.HasMetadata && attempts < 5) // wait and see if we can fetch torrent file for 5~ minutes before adding next magnet
                        {
                            attempts++;
                            Wait(60);
                        }
                        FilePaths.Remove(f);
                        File.Delete(f);
                    }
                }

                SetFileHandlerStatusFromWatcherStatus();

            }
            else
            {
                Wait(121);
                await ProcessFileQueue();
            }

        }

        private async void TorrentStateChanged(object sender, TorrentStateChangedEventArgs e)
        {

            if (e.OldState == TorrentState.Metadata && e.NewState == TorrentState.Starting)
            {
                await e.TorrentManager.StopAsync();
                await ClientEngine.Unregister(e.TorrentManager);
                e.TorrentManager.Dispose();
            }

        }

        private MagnetLink GetMagnetLink(string f)
        {
            if (File.Exists(f))
            {
                var fileText = File.ReadAllText(f);
                MagnetLink ml = MagnetLink.Parse(fileText);
                return ml;
            }
            return null;
        }

        private void Wait(int seconds)
        {
            Thread.Sleep(seconds * 1000);
        }


    }

    public enum FileHandlerStatus
    {
        WatcherStopped,
        IdleWatching,
        HandlingFiles,
        ProcessingQueue
    }
}
