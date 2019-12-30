// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Novacode;
using Platest.Helpers;
using Platest.Interfaces;
using Platest.Models;
using Image = System.Drawing.Image;

namespace Platest.Controllers
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
        private readonly ISourceLoadListener _listener;
        private string _fileName;

        public SourceController(ISourceLoadListener listener)
        {
            _listener = listener;
        }

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
            var result = e.Result as SourceFile;
            _listener.OnSourceLoaded(result);
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            _fileName = e.Argument as string;
            if (_fileName == null)
            {
                throw new NullReferenceException("Имя файла равно null");
            }

            var extension = Path.GetExtension(_fileName);
            SourceFile file = null;
            switch (extension)
            {
                case ".txt":
                    file = GetTxt(_fileName);
                    break;

                case ".docx":
                case ".doc":
                    file = GetDocXText(_fileName);
                    break;
            }
            if (file != null)
            {

            }
            e.Result = file;
        }

        /// <summary>
        /// Если был передан обыкновенный текстовый файл
        /// </summary>
        private SourceFile GetTxt(string filename)
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
                // MessageBox.Show($"Проблема при открытии файла вопросов: {ex.Message}");
            }
            finally
            {
                reader?.Close();
                reader?.Dispose();
            }
            return new SourceFile(text, null, Path.GetFileName(filename));
        }

        /// <summary>
        /// Эта функция берет файл и создает документ-контейнер для него.
        /// Это занимает некоторое время, что вынуждает использовать раздельные потоки
        /// </summary>
        private SourceFile GetDocXText(string filename)
        {
            try
            {
                var document = DocX.Load(filename);
                var text = string.Empty;
                text = text
                    .ReplaceImages(document)
                    .ReplaceTables(document);
                //
                var images = document.Images;
                text = text
                    .Replace("<question>", "\r\n<question>")
                    .Replace("<variant>", "\r\n<variant>");

                return new SourceFile(text, images, Path.GetFileName(filename));
            }
            catch (Exception ex)
            {
                // MessageBox.Show($"Возникла ошибка при открытии файла:\n{ex.Message}");
            }
            return null;
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
    }
}