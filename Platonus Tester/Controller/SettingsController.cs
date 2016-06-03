using System;
using System.IO;
using System.Windows;
using System.Xml.Serialization;
using Platonus_Tester.Helper;

namespace Platonus_Tester.Controller
{
    public abstract class SettingsController
    {
        private const string FileName = "settings.xml";

        public static void SaveSettings(Settings settings)
        {
            using (var stream = new StreamWriter(File.Open(FileName, FileMode.Create)))
            {
                var serializer = new XmlSerializer(typeof(Settings));
                serializer.Serialize(stream, settings);
            }
        }

        public static Settings Load()
        {
            StreamReader stream = null;
            try
            {
                if (File.Exists(FileName))
                {
                    stream = new StreamReader(File.Open(FileName, FileMode.Open));
                    var serializer = new XmlSerializer(typeof(Settings));
                    var returnSettings = serializer.Deserialize(stream) as Settings;
                    return returnSettings;
                }
                else
                {
                    var settings = new Settings();
                    SaveSettings(settings);
                    return settings;
                }
            }
            catch (Exception ex)
            {

                define_error("Ошибка 4.2\nЗагружены настройки по умолчанию " + ex.Message);

            }
            finally
            {
                stream?.Close();
            }
            return new Settings();
        }

        private static void define_error(string text)
        {
            MessageBox.Show(text);
        }
    }
}