using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MagnetLinkConverter.Utility
{
    public static class SettingsHelper
    {
        private static readonly string _fileName = "magnetlinkconverter_settings.xml";

        private static string FilePath { get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _fileName); } }

        private const string _xmlMagnetLinkConverter = "magnetlinkconverter";
        private const string _xmlMagnetPath = "magnetpath";
        private const string _xmlTorrentFilePath = "torrentfilepath";


        private static SettingsValues _values;
        public static SettingsValues Values
        {
            get
            {
                if (_values == null)
                    _values = ReadSettings();
                return _values;
            }
        }

        public static void WriteSettings()
        {
            using (XmlWriter writer = XmlWriter.Create(FilePath))
            {
                writer.WriteStartElement(_xmlMagnetLinkConverter);
                writer.WriteElementString(_xmlMagnetPath, Values.MagnetPath);
                writer.WriteElementString(_xmlTorrentFilePath, Values.TorrentFilePath);
                writer.WriteEndElement();
                writer.Flush();
            }
        }

        private static SettingsValues ReadSettings()
        {

            var values = new SettingsValues();
            if (File.Exists(FilePath))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(_fileName);
                XmlNode node = doc.DocumentElement;

                foreach (XmlNode child in node.ChildNodes)
                {
                    switch (child.Name)
                    {
                        case _xmlMagnetPath:
                            values.MagnetPath = child.InnerText;
                            break;
                        case _xmlTorrentFilePath:
                            values.TorrentFilePath = child.InnerText;
                            break;
                    }
                }

            }
            return values;
        }

    }




    public class SettingsValues
    {
        public string MagnetPath { get; set; }
        public string TorrentFilePath { get; set; }

        public bool AllSettingsOk()
        {
            return Directory.Exists(MagnetPath) && Directory.Exists(TorrentFilePath);
        }
    }
}
