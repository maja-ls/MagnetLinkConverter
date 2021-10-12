using MagnetLinkConvertForms.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MagnetLinkConvertForms.Code
{
    public class FileHandler
    {
        private FileSystemWatcher Watcher { get; }
        private HashSet<string> FilePaths { get; set; }

        private static object _lock = new object();
        private static bool _isLocked = false;
        public FileHandler()
        {

            Watcher = new FileSystemWatcher();

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
                lock (_lock)
                {
                    _isLocked = true;

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
            } else
            {
                Wait(2);
                HandleAllMagnetFilesInDirectory();
            }
        }

        private void ProcessFileQueue()
        {
            if (!_isLocked)
            {
                lock(_lock)
                {
                    _isLocked = true;

                    var pathList = FilePaths.ToList();
                    foreach(var f in pathList)
                    {
                        // DO TORRENT SHIT HERE!!!

                        // remove path from FilePaths
                        FilePaths.Remove(f);
                    }
                }
                _isLocked = false;
            } else
            {
                Wait(2);
                ProcessFileQueue();
            }

        }

        private void Wait(int minutes)
        {
            Thread.Sleep(minutes * 1000);
        }


    }
}
