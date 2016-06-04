using System.Collections.Generic;

namespace Platonus_Tester.Model
{
    /// <summary>
    /// Класс-обертка для исходного файла
    /// </summary>
    public class SourceFile
    {
        /// <summary>
        /// текст документа. Включая таблицы и теги картинок
        /// </summary>
        public string SourceText        { get; set;  }
        /// <summary>
        /// Массив картинок в формате библиотеки Novacode
        /// </summary>
        public List<Novacode.Image> Images { get; set;  }
        /// <summary>
        /// Имя документа в файл-системе для отобрадения в UI компонентах
        /// </summary>
        public string FileName { get; set; }

        public SourceFile(string text, List<Novacode.Image> images)
        {
            SourceText = text;
            Images = images;
            FileName = "";
        }
    }
}