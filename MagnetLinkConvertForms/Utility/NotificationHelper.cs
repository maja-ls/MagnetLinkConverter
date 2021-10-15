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
        private FileHandler fileHandler;
        private NotifyIcon trayIcon;

        public NotificationHelper(FileHandler fileHandler, System.Windows.Forms.NotifyIcon trayIcon)
        {
            this.fileHandler = fileHandler;
            this.trayIcon = trayIcon;
        }
    }
}
