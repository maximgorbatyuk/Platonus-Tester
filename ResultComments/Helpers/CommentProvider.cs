using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ResultComments.Models;

namespace ResultComments.Helpers
{
    /// <summary>
    /// Обработчик скачанного текста с репозитория в массив строк
    /// </summary>
    public abstract class CommentProvider
    {

        public static List<string> GetHashList(string text)
        {
            var result = new List<string>(0);
            var splitSeparators = new[] { "#\n" };
            var list = text.Split(splitSeparators, StringSplitOptions.RemoveEmptyEntries);
            result.AddRange(list);
            return result;
        }
    }
}