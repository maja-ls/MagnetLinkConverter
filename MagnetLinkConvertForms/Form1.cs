using MagnetLinkConverter.Code;
using MagnetLinkConverter.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MagnetLinkConverter
{
    public partial class Form1 : Form
    {

        private string MagnetPath { get; set; }
        private string TorrentPath { get; set; }
        private Action UpdateFileWatcher { get; }

        private NotificationHelper NotificationHelper { get; }

        public Form1(NotificationHelper notificationHelper, Action updateFileWatcher)
        {
            InitializeComponent();
            txtbox_magnetpath.Enabled = false;
            txtbox_torrentpath.Enabled = false;

            var settings = SettingsHelper.Values;

            MagnetPath = settings.MagnetPath;
            TorrentPath = settings.TorrentFilePath;
            txtbox_magnetpath.Text = MagnetPath;
            txtbox_torrentpath.Text = TorrentPath;

            UpdateFileWatcher = updateFileWatcher;
        }

        private void SaveSettings()
        {
            bool changed = false;
            if (SettingsHelper.Values.MagnetPath != MagnetPath)
            {
                changed = true;
                SettingsHelper.Values.MagnetPath = MagnetPath;
            }
            if (SettingsHelper.Values.TorrentFilePath != TorrentPath)
            {
                changed = true;
                SettingsHelper.Values.TorrentFilePath = TorrentPath;
            }

            if (changed)
            {
                SettingsHelper.WriteSettings();
                UpdateFileWatcher();
            }
        }


        private string GetDirectoryPath(string fallback)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    return fbd.SelectedPath;
                }
                else
                {
                    return fallback;
                }
            }
        }



        private void magnetlink_Path_Click(object sender, EventArgs e)
        {
            string path = GetDirectoryPath(MagnetPath);
            txtbox_magnetpath.Text = path;
            MagnetPath = path;
            SaveSettings();
        }

        private void torrentFile_Path_Click(object sender, EventArgs e)
        {
            string path = GetDirectoryPath(TorrentPath);
            txtbox_torrentpath.Text = path;
            TorrentPath = path;
            SaveSettings();
        }


        private void btnOpenMagnetPath_Click(object sender, EventArgs e)
        {
            OpenDirInExplorer(MagnetPath);
        }

        private void btnOpenTorrentFilePath_Click(object sender, EventArgs e)
        {
            OpenDirInExplorer(TorrentPath);
        }

        private void OpenDirInExplorer(string path)
        {
            if (Directory.Exists(path))
                System.Diagnostics.Process.Start(Environment.GetEnvironmentVariable("WINDIR") + @"\explorer.exe", @path);
        }

    }
}
