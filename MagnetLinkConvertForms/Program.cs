using MagnetLinkConvertForms.Code;
using MagnetLinkConvertForms.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MagnetLinkConvertForms
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
            Application.Run(new MyCustomApplicationContext());
        }

        public class MyCustomApplicationContext : ApplicationContext
        {
            private NotifyIcon TrayIcon { get; }
            private Form1 Form { get; }
            private FileHandler FileHandler{ get; }

            public MyCustomApplicationContext()
            {
                // Initialize Tray Icon
                TrayIcon = new NotifyIcon();

                TrayIcon.Icon = MagnetLinkConvertForms.Properties.Resources.magnet;

                TrayIcon.ContextMenuStrip = PopulateContextMenu();
                TrayIcon.Text = "Trayicon text";
                //trayIcon.BalloonTipText = "Trayicon balloontext";
                TrayIcon.Visible = true;
                //trayIcon.Click += (object sender, EventArgs e) => { trayIcon.ShowBalloonTip(1000); };
                TrayIcon.Click += HandleTrayClick;
                TrayIcon.DoubleClick += HandleTrayDoubleClick;

                FileHandler = new FileHandler();
                Form = new Form1(FileHandler);
            }

            private void HandleTrayDoubleClick(object sender, EventArgs e)
            {
                Form.Show();
            }

            private void HandleTrayClick(object sender, EventArgs e)
            {
                //MessageBox.Show("Hello world");

            }

            private ContextMenuStrip PopulateContextMenu()
            {
                var strip = new ContextMenuStrip();
                strip.Items.Add("Exit", null, Exit);
                return strip;
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
