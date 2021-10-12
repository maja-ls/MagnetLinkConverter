using MagnetLinkConvertForms.Code;
using MagnetLinkConvertForms.Utility;
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

namespace MagnetLinkConvertForms
{
    public partial class Form1 : Form
    {

        private string MagnetPath { get; set; }
        private string TorrentPath { get; set; }
        private string DownloadPath { get; set; }

        private FileHandler FileHandler { get; }

        public Form1(FileHandler fileHandler)
        {
            InitializeComponent();
            FileHandler = fileHandler;
            txtbox_downloadpath.Enabled = false;
            txtbox_magnetpath.Enabled = false;
            txtbox_torrentpath.Enabled = false;

            var settings = SettingsHelper.Values;

            MagnetPath = settings.MagnetPath;
            TorrentPath = settings.TorrentFilePath;
            DownloadPath = settings.TorrentDownloadingPath;
            txtbox_magnetpath.Text = MagnetPath;
            txtbox_torrentpath.Text = TorrentPath;
            txtbox_downloadpath.Text = DownloadPath;
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
            if (SettingsHelper.Values.TorrentDownloadingPath != DownloadPath)
            {
                changed = true;
                SettingsHelper.Values.TorrentDownloadingPath = DownloadPath;
            }
            if (changed)
            {
                SettingsHelper.WriteSettings();
                FileHandler.UpdateWatcher();
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
        private void download_Path_Click(object sender, EventArgs e)
        {
            string path = GetDirectoryPath(DownloadPath);
            txtbox_downloadpath.Text = path;
            DownloadPath = path;
            SaveSettings();

        }
    }
}
