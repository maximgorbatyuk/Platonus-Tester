namespace Platest.Helpers
{
    public static class Extensions
    {
        /// <summary>
        /// Функция для поиска и замены изображений в документе
        /// Пробегаюсь по параграфам и нахожу картинки
        /// Картинки имею свое имя. Обычно один параграф и есть одна картинка, что упрощает поиск
        /// Чтобы в дальнейшем запомнить положение той или иной картинки, я 
        /// Заменяю место в тексте специальным тегом #picture \\name\\
        /// В дальнейшем при обработке я ищу эти теги и заменяю картинками по имени
        /// </summary>
        /// <param name="text"></param>
        /// <param name="doc"></param>
        /// <returns>Полный текст документа + теги картинок</returns>
        public static string ReplaceImages(this string text, Novacode.Container doc)
        {
            var newText = string.Empty;
            foreach (var p in doc.Paragraphs)
            {
                newText += p.Text;
                if (p.Pictures.Count != 0)
                {
                    newText += $"<#picture= {p.Pictures[0].FileName} #>";
                }
            }
            return newText;
        }

        /// <summary>
        /// Функция для замены таблиц в документе текстовым аналогом 
        /// | столбец | столбец |
        /// | строка  | строка  |
        /// Выравнивание пока не реализовано, но пока и так вариант ответа понятен
        /// Каждая ячейка в исходном документе была отдельным параграфом, поэтому я передаю и исходный текст,
        /// чтобы заменить эти ошибки на презентабельный вид
        /// </summary>
        /// <param name="text">текст с картинками</param>
        /// <param name="doc">Контейнер документа</param>
        /// <returns>Полный текст (с картинками) + Таблицы</returns>
        public static string ReplaceTables(this string text, Novacode.Container doc)
        {

            foreach (var p in doc.Tables)
            {
                var processedText = string.Empty;
                var replaceText = string.Empty;
                var look = p.Paragraphs;

                for (var i = 0; i < look.Count; i++)
                {
                    if (i != 0 && i % p.ColumnCount == 0)
                    {
                        processedText += "\n";
                    }
                    replaceText += look[i].Text;
                    processedText += $"| {look[i].Text} ";

                }

                text = text.Replace(replaceText, processedText);
                //processedText += "\n";
            }
            return text;
        }
    }
}