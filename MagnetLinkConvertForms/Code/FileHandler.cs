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

        private static object _lock = new object();
        private static bool _isLocked = false;
        public FileHandler()
        {
            FilePaths = new HashSet<string>();
            Watcher = new FileSystemWatcher();
            ClientEngine = new ClientEngine(new EngineSettings());

            Watcher.Path = SettingsHelper.Values.MagnetPath;
            Watcher.IncludeSubdirectories = true;
            Watcher.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastWrite;
            Watcher.Filter = "*.magnet";
            Watcher.Changed += new FileSystemEventHandler(OnChanged);
            Watcher.Created += new FileSystemEventHandler(OnChanged);

            HandleAllMagnetFilesInDirectory();
        }

        public void StartWatcher()
        {
            if (SettingsHelper.Values.AllSettingsOk())
            {
                Watcher.EnableRaisingEvents = true;
                HandleAllMagnetFilesInDirectory();
            }
        }
        public void StopWatcher()
        {
            Watcher.EnableRaisingEvents = false;
        }

        public void UpdateWatcher()
        {
            StopWatcher();
            Watcher.Path = SettingsHelper.Values.MagnetPath;
            StartWatcher();
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            HandleAllMagnetFilesInDirectory();
        }

        private void HandleAllMagnetFilesInDirectory()
        {
            if (!_isLocked)
            {
                _isLocked = true;
                lock (_lock)
                {

                    var magnetfiles = Directory.GetFiles(SettingsHelper.Values.MagnetPath, "*.magnet");
                    foreach (var f in magnetfiles)
                    {
                        if (!FilePaths.Contains(f))
                        {
                            FilePaths.Add(f);
                        }
                    }

                }
                _isLocked = false;
                ProcessFileQueue();
            }
            else
            {
                Wait(2);
                HandleAllMagnetFilesInDirectory();
            }
        }

        private void ProcessFileQueue()
        {
            if (!_isLocked)
            {
                _isLocked = true;
                lock (_lock)
                {

                    var pathList = FilePaths.ToList();
                    if (pathList.Count > 0)
                    {

                        foreach (var f in pathList)
                        {
                            // DO TORRENT SHIT HERE!!!
                            MagnetLink ml = GetMagnetLink(f);
                            TorrentManager manager = new TorrentManager(ml, SettingsHelper.Values.TorrentDownloadingPath, new TorrentSettings(), SettingsHelper.Values.TorrentFilePath);
                            ClientEngine.Register(manager);
                            manager.StartAsync();
                            manager.TorrentStateChanged += TorrentStateChanged;

                            // remove path from FilePaths
                            FilePaths.Remove(f);
                            File.Delete(f);
                        }
                    }
                }
                _isLocked = false;
            }
            else
            {
                Wait(2);
                ProcessFileQueue();
            }

        }

        private async void TorrentStateChanged(object sender, TorrentStateChangedEventArgs e)
        {

            if(e.OldState == TorrentState.Metadata && e.NewState == TorrentState.Starting)
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

        private void Wait(int minutes)
        {
            Thread.Sleep(minutes * 1000);
        }


    }
}
