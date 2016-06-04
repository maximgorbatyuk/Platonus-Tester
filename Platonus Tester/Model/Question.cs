using System.Collections.Generic;
using Image = System.Drawing.Image;

namespace Platonus_Tester.Model
{
    /// <summary>
    /// Класс-предок для отвеченного вопроса и незаданного
    /// </summary>
    public class Question
    {
        /// <summary>
        /// Вопрос
        /// </summary>
        public string AskQuestion { get; set; }
        /// <summary>
        /// Изображение. Может быть NULL
        /// </summary>
        public Image Picture { get; set; }
        /// <summary>
        /// Массив вариантов ответа
        /// </summary>
        public List<string> AnswerList { get; set; }
        
    }
}