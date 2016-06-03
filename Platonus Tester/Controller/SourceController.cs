using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Novacode;
using Platonus_Tester.CustomArgs;
using Platonus_Tester.Model;
using Image = System.Drawing.Image;

namespace Platonus_Tester.Controller
{
    public class SourceController
    {
        public delegate void SourceFileLoadCompleted(object sender, SourceFileLoadedArgs e);
        public event SourceFileLoadCompleted OnLoadComleted;
        private string _fileName;
        private Thread _thread;

        public async Task<SourceFile> ProcessSourceFileAsync(string fileName)
        {
            _fileName = fileName;
            SourceFile result = null;
            if (_fileName.IndexOf(".txt", StringComparison.Ordinal) > -1)
            {
                result = GetTXT(fileName);
                //DefineResult(GetTXT(_fileName));
            }

            if (_fileName.IndexOf(".docx", StringComparison.Ordinal) > -1 ||
                _fileName.IndexOf(".doc", StringComparison.Ordinal) > -1
                )
            {
                result = GetDocXText(fileName);
                // DefineResult(GetDocXText(_fileName));
            }
            await Task.Delay(50);

            //_thread = new Thread(StartProcessing);
            // _thread.Start();
            return result;
        }

        private void StartProcessing()
        {
            
        }

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
            }
            return new SourceFile(text, null);
        }

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

        private string ReplaceImages(string text, Container doc)
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

        private string ReplaceTables(string text, Container doc)
        {
            // TODO: 
            // Here is table text processing. Need to be debugged
            // Also I didn't feagure out about replacing tables in main text

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

        private void DefineResult(SourceFile text)
        {
            if (OnLoadComleted == null) return;
            //_thread.Abort();
            var args = new SourceFileLoadedArgs(text);
            OnLoadComleted(this, args);
        }
    }
}