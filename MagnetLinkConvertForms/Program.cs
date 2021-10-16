using MagnetLinkConverter.Code;
using MagnetLinkConverter.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MagnetLinkConverter
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        /// 
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MagnetLinkConverterContext());
        }

        public class MagnetLinkConverterContext : ApplicationContext
        {
            private NotifyIcon TrayIcon { get; }
            private FileHandler FileHandler { get; }
            private NotificationHelper NotificationHelper { get; }

            public MagnetLinkConverterContext()
            {


                // Initialize Tray Icon
                TrayIcon = new NotifyIcon();
                InitTrayIcon();

                NotificationHelper = new NotificationHelper(TrayIcon);

                FileHandler = new FileHandler(NotificationHelper);
                FileHandler.StartWatcher();

            }

            private void HandleTrayDoubleClick(object sender, EventArgs e)
            {
                Form1 form = new Form1(NotificationHelper, FileHandler.UpdateWatcher);

                form.Show();
            }

            private void HandleTrayClick(object sender, EventArgs e)
            {
                //MessageBox.Show("Hello world");

            }

            private void InitTrayIcon()
            {
                TrayIcon.Icon = MagnetLinkConverter.Properties.Resources.magnet;

                var strip = new ContextMenuStrip();
                strip.Items.Add("Quit", null, Exit);

                TrayIcon.ContextMenuStrip = strip;
                TrayIcon.Visible = true;
                TrayIcon.Click += HandleTrayClick;
                TrayIcon.DoubleClick += HandleTrayDoubleClick;
            }

            void Exit(object sender, EventArgs e)
            {
                // Hide tray icon, otherwise it will remain shown until user mouses over it
                TrayIcon.Visible = false;

                Application.Exit();
            }


        }
    }
}
