// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Platonus_Tester.Model;

namespace Platonus_Tester.Helper
{
    /// <summary>
    /// Класс-кор проекта. Обработчик текста вопросов в хэш вопросов-объектов
    /// </summary>
    public class QuestionProcessor
    {
        private SourceFile _file;
        private List<string> _errorList;

        public QuestionProcessor()
        {
            _errorList = new List<string>(0);
        }

        /// <summary>
        /// Функция-инициализатор обработки. Определяю количество вхождений слова question в тегах 
        /// для того, чтобы определить количество итераций. 
        /// </summary>
        /// <param name="file">Объект содержит текст и картинки</param>
        /// <returns>Массив тестовых вопросов</returns>
        public List<TestQuestion> GetQuestionList(SourceFile file)
        {
            _file = file;
            var text = file.SourceText;
            var result = new List<TestQuestion>(0);
            text = ProcessText(text);

            var questionCount = GetWordCount("<question>", text);
            for (var i = 0; i < questionCount; i++)
            {
                text = text.Substring(GetIndex("<question>", text) + 10);
                var endPos = GetIndex("<question>", text);
                var questionText = text.Substring(0, endPos > -1 ? GetIndex("<question>", text) : text.Length);

                if (endPos > -1)
                    text = text.Substring(endPos);

                var question = GetQuestion(questionText);
                if (question != null)
                {
                    result.Add(question);
                }
                else
                {
                    // MessageBox.Show("Возникла ошибка на этапе обработки вопроса " + i);
                }

            }
            // ShowErrors();
            return result;
        }

        /// <summary>
        /// Здесь планирую сделать замену TAMOS формата в Platonus формат
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static string ProcessText(string text)
        {
            /*
             * TODO:
             * TestQuestion > [q]3:1:
             * variant > [a] ( variant > [a]+)
             * 
             */

            var result = text.Replace("\t", "");

            return result;
        }

        /// <summary>
        /// Функция, возвращающая тестовый вопрос
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private TestQuestion GetQuestion(string text)
        {
            try
            {
                var varCount = GetWordCount("<variant>", text);

                var quest = text.Substring(0, GetIndex("\r\n<variant>", text));
                var image = GetImage(quest);
                quest = RemovePictureText(quest);
                text = text.Substring(GetIndex("<variant>", text));

                var hash = new List<string>(0);

                for (var j = 0; j < varCount; j++)
                {
                    try
                    {
                        text = text.Substring(GetIndex("<variant>", text) + "<variant>".Length);
                        var pos = GetIndex("\r\n", text);
                        var variant = "";
                        if (pos != -1)
                        {
                            variant = text.Substring(0, pos);
                        }
                        else
                        {
                            // 
                            if (GetWordCount("<variant>", text) == 0 && text.Length > 0)
                                variant = text;
                        }
                        hash.Add(variant);
                        text = text.Substring(pos + 2);
                    }
                    catch (Exception ex)
                    {
                        _errorList.Add("Проблема в обработке вопроса: \r\n" + quest + "\r\n" + ex.Message);
                    }
                }

                for (var i = hash.Count; i < 5; i++)
                {
                    hash.Add(Const.MissingVariant);
                }

                var result = new TestQuestion
                {
                    AskQuestion = quest,
                    CorrectAnswer = hash[0],
                    AnswerList = hash,
                    Picture = image
                };
                return result;
            }
            catch (Exception ex)
            {
                _errorList.Add($"Проблема в обработке вопроса: \r\n{text}\r\n{ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Поиск и удаление всех вхождений тегов picture
        /// </summary>
        /// <param name="text">Исходный текст для форматирования</param>
        /// <returns>Текст без тегов картинок</returns>
        private string RemovePictureText(string text)
        {
            if (GetIndex("<#picture= ", text) == -1)
            {
                return text;
            }

            var picName = GetPictureName(text);
            var result = text.Replace($"<#picture= {picName} #>", "");
            return result;
        }

        private string GetPictureName(string text)
        {
            try
            {
                var index = GetIndex("<#picture= ", text);
                var size = "<#picture= ".Length;
                var picName = text.Substring(index + size);
                picName = picName.Substring(0, GetIndex(" #>", picName));
                return picName;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Функция - вычислитель количество вхождений слова в тексте
        /// </summary>
        /// <param name="word">Искомое слово</param>
        /// <param name="source">Текст, в котором ищется слово</param>
        /// <returns>Количество вхождений</returns>
        public int GetWordCount(string word, string source)
        {
            return (source.Length - source.Replace(word, "").Length) / word.Length;
        }

        public List<string> GetErrors()
        {
            if (_errorList == null || _errorList.Count <= 0) return null;
            var tmp = _errorList;
            _errorList = new List<string>(0);
            return tmp;
        }

        private Image GetImage(string question)
        {
            if (GetIndex("<#picture= ", question) == -1)
            {
                return null;
            }

            var picName = GetPictureName(question);
            return FindImageByName(picName);
        }

        private int GetIndex(string search, string text)
        {
            return text.IndexOf(search, StringComparison.Ordinal);
        }

        /// <summary>
        /// Поиск и возврат картинки по названию
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private Image FindImageByName(string name)
        {
            Image result = null;
            foreach (var img in _file.Images)
            {
                if (name != img.FileName) continue;
                result = Image.FromStream(img.GetStream(FileMode.Open, FileAccess.Read));
                break;
            }
            return result;
        }
    }
}