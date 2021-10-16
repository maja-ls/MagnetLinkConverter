using MagnetLinkConverter.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MagnetLinkConverter.Utility
{
    public class NotificationHelper
    {
        private NotifyIcon trayIcon;
        private FileHandlerStatus lastStatus;

        public NotificationHelper(System.Windows.Forms.NotifyIcon trayIcon)
        {
            this.trayIcon = trayIcon;
        }

        internal void UpdateFileHandlerStatus(FileHandlerStatus status)
        {
            lastStatus = status;
            UpdateTrayText();
        }

        private void UpdateTrayText()
        {
            var s = "";
            switch (lastStatus)
            {
                case FileHandlerStatus.WatcherStopped:
                    {
                        if (SettingsHelper.Values.AllSettingsOk())
                            s = GetStringMax64Chars("Filewatcher is not active", "");
                        else
                            s = GetStringMax64Chars("Filewatcher is not active - Settings need attention", "");
                        break;
                    }
                case FileHandlerStatus.IdleWatching:
                    {
                        s = GetStringMax64Chars("Watching for changes", SettingsHelper.Values.MagnetPath);
                        break;
                    }
                case FileHandlerStatus.HandlingFiles:
                    {
                        s = GetStringMax64Chars("Changes detected", SettingsHelper.Values.MagnetPath);
                        break;
                    }
                case FileHandlerStatus.ProcessingQueue:
                    {
                        s = GetStringMax64Chars("Processing files", SettingsHelper.Values.MagnetPath);
                        break;
                    }
            }
            trayIcon.Text = s;
        }

        private string GetStringMax64Chars(string mandatory, string folderPath)
        {
            var mandatorySubstringed = mandatory.Length < 65 ? mandatory : mandatory.Substring(0, 64);
            if (string.IsNullOrEmpty(folderPath))
            {
                return mandatorySubstringed;
            }
            else
            {
                var combineWithPath = " in: ";
                var stringLengthWithoutPath = mandatory.Length + combineWithPath.Length;
                if (stringLengthWithoutPath < 64)
                {
                    // find the longest possible path to return
                    if ((stringLengthWithoutPath + folderPath.Length) <= 64)
                    {
                        return $"{mandatory}{combineWithPath}{folderPath}";
                    }
                    else
                    {
                        // can't return full path, calculate longest possible to return
                        string prefix = "...";
                        stringLengthWithoutPath = stringLengthWithoutPath + prefix.Length;
                        string currentReturnPath = "";
                        var slashes = new char[] { '\\', '/' };

                        var indexOfSlash = folderPath.LastIndexOfAny(slashes);
                        string calculated = folderPath.Substring(indexOfSlash);


                        while ((stringLengthWithoutPath + calculated.Length) <= 64)
                        {
                            //can return at least one folder, try if we can return more
                            currentReturnPath = calculated;
                            indexOfSlash = folderPath.LastIndexOfAny(slashes, indexOfSlash - 1);
                            calculated = folderPath.Substring(indexOfSlash);
                        }

                        if (!string.IsNullOrEmpty(currentReturnPath))
                        {
                            return $"{mandatory}{combineWithPath}{prefix}{currentReturnPath}";
                        }
                        else
                        {
                            return mandatorySubstringed;
                        }

                    }
                }
                else
                {
                    return mandatorySubstringed;
                }
            }
        }
    }
}
