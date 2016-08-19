using System;
using System.IO;
using System.Windows;
using System.Xml.Serialization;
using Platonus_Tester.Helper;

namespace Platonus_Tester.Controller
{
    /// <summary>
    /// Класс для управления настройками. Настройки записываются в XML формате в файл в той же директории
    /// </summary>
    public abstract class SettingsController
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settings"></param>
        public static void SaveSettings(Settings settings)
        {
            var fileName = $"{Environment.CurrentDirectory}\\settings.xml";
            StreamWriter stream = null;
            try
            {
                stream = new StreamWriter(File.Open(fileName, FileMode.Create));
                var serializer = new XmlSerializer(typeof(Settings));
                serializer.Serialize(stream, settings);
            }
            catch (Exception ex)
            {
                //ignored
            }
            finally
            {
                stream?.Close();
            }
        }

        /// <summary>
        /// Загрузка настроек из файла. Если происходит ошибка, возвращаются станлартные настройки
        /// </summary>
        /// <returns></returns>
        public static Settings Load()
        {
            var fileName = $"{Environment.CurrentDirectory}\\settings.xml";
            Settings settings = null;
            StreamReader stream = null;
            try
            {
                if (File.Exists(fileName))
                {
                    stream = new StreamReader(File.Open(fileName, FileMode.Open));
                    var serializer = new XmlSerializer(typeof(Settings));
                    settings = serializer.Deserialize(stream) as Settings;
                }
                else
                {
                    settings = new Settings();
                    SaveSettings(settings);
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
            return settings;
        }

        private static void define_error(string text)
        {
            MessageBox.Show(text);
        }
    }
}