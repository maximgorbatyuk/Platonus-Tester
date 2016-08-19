using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Novacode;
using Platonus_Tester.CustomArgs;
using Platonus_Tester.Model;
using Container = Novacode.Container;
using Image = System.Drawing.Image;

namespace Platonus_Tester.Controller
{
    /// <summary>
    /// Класс для управления входным файлом. За основу взята библиотека 
    /// Novacode.DocX, как бесплатная и легкодоступная. Из проблем библиотеки
    /// стоит отметить то, что работает она в одном потоке, и в ней нет функций async / await
    /// Это значит, что мне нужно организовать работу в фоне, так как загрузка файла docx/doc
    /// Может занимать достаточное для глаза время блокировки UI треда.
    /// Например, тест по ИнфБез открывается около 6 секунд
    /// </summary>
    public class SourceController
    {
        /// <summary>
        /// Здесь я определяю реализацию паттерна Listenter (Observer)
        /// Когда заканчивается обработка документа, вызывается событие окончания, 
        /// и данные в UI потоке обновляются
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void SourceFileLoadCompleted(object sender, SourceFileLoadedArgs e);
        public event SourceFileLoadCompleted OnLoadComleted;
        private string _fileName;
        private Thread _thread;

        /// <summary>
        /// Инициализация. Здесь же определяю и Background worker
        /// Об этой функции узнал недавно (04.06.2016), так как были проблемы с Thread.Start
        /// Создание отедльного треда вынудило использовать кучу костылей в обновлении UI компонентов, 
        /// Доходило до того, что я и компонент, и переменную для обновления (внутри лямбда функции) 
        /// вызывал через Dispatcher
        /// Вывод: в WinForm создание треда было проще (или просто мне понятней)
        /// </summary>
        /// <param name="fileName"></param>
        public void ProcessSourceFileAsync(string fileName)
        {
            _fileName = fileName;

            var worker = new BackgroundWorker();
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync(fileName);


            //_thread = new Thread(StartProcessing);
            // _thread.Start();
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var result = (SourceFile) e.Result;
            DefineResult(result);
            // throw new NotImplementedException();
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            _fileName = (string) e.Argument;
            SourceFile file = null;
            if (_fileName.Contains(".txt"))
            {
                file =  GetTXT(_fileName);
                //DefineResult(GetTXT(_fileName));
            }

            if (_fileName.Contains(".docx") ||
                _fileName.Contains(".doc") )
            {
                file = GetDocXText(_fileName);
                //DefineResult(GetDocXText(_fileName));
            }
            if (file != null)
            {
                var pos = _fileName.LastIndexOf("\\", StringComparison.Ordinal);
                pos = pos != -1 ? pos + 1 : 0;
                file.FileName = _fileName.Substring(pos);
            }
            e.Result = file;
        }

        /// <summary>
        /// Если был передан обыкновенный текстовый файл
        /// </summary>
        private SourceFile GetTXT(string filename)
        {
            StreamReader reader = null;
            string text = null;
            try
            {
                reader = new StreamReader(filename, Encoding.Default);
                text = reader.ReadToEnd();
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Проблема при открытии файла вопросов: {ex.Message}");
            }
            finally
            {
                reader?.Close();
                reader?.Dispose();
            }
            return new SourceFile(text, null);
        }

        /// <summary>
        /// Эта функция берет файл и создает документ-контейнер для него.
        /// Это занимает некоторое время, что вынуждает использовать раздельные потоки
        /// </summary>
        private SourceFile GetDocXText(string filename)
        {
            var document = DocX.Load(filename);
            var text = "";
            text = ReplaceImages(text, document);
            text = ReplaceTables(text, document);
            //
            var images = document.Images;
            text = text.Replace("<question>", "\r\n<question>");
            text = text.Replace("<variant>", "\r\n<variant>");


            return new SourceFile(text, images);
        }

        private List<Image> ConvertImages(IEnumerable<Novacode.Picture> pictures, IEnumerable<Novacode.Image> images)
        {
            var result = new List<Image>(0);
            var enumerable = images as Novacode.Image[] ?? images.ToArray();
            foreach (var pic in pictures)
            {
                Novacode.Image image = null;
                foreach (var img in enumerable)
                {
                    if (pic.FileName != img.FileName) continue;
                    image = img;
                    break;
                }
                if (image == null) continue;
                result.Add(Image.FromStream(image.GetStream(FileMode.Open, FileAccess.Read)));
            }
            return result;
        }

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
        private static string ReplaceImages(string text, Container doc)
        {
            var newText = "";
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
        private static string ReplaceTables(string text, Container doc)
        {

            foreach (var p in doc.Tables)
            {
                var processedText = "";
                var replaceText = "";
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

        /// <summary>
        /// Событие, создающее возникновения события обработки
        /// </summary>
        /// <param name="text"></param>
        private void DefineResult(SourceFile text)
        {
            if (OnLoadComleted == null) return;
            //_thread.Abort();
            //_thread.Abort();
            var args = new SourceFileLoadedArgs(text);
            OnLoadComleted(this, args);
        }
    }
}