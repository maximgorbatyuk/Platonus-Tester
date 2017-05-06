using System.Collections.Generic;

namespace Platest.Models
{
    /// <summary>
    /// Класс-обертка для исходного файла
    /// </summary>
    public class SourceFile
    {
        /// <summary>
        /// текст документа. Включая таблицы и теги картинок
        /// </summary>
        public string SourceText { get; }
        /// <summary>
        /// Массив картинок в формате библиотеки Novacode
        /// </summary>
        public List<Novacode.Image> Images { get; }
        /// <summary>
        /// Имя документа в файл-системе для отобрадения в UI компонентах
        /// </summary>
        public string FileName { get; }

        public SourceFile(string text, List<Novacode.Image> images, string filename)
        {
            SourceText = text;
            Images = images;
            FileName = filename;
        }
    }
}