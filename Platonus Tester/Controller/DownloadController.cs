using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Platonus_Tester.Controller
{
    /// <summary>
    /// Класс-помощник для загрузки новых комментариев с репозитория на GitHub
    /// Операции выполняются асинхронно
    /// </summary>
    public abstract class DownloadController
    {

        public static async Task<string> ExecuteRequestAsync(string url)
        {
            try
            {
                if (url == "") return null;
                var downloader = new WebClient
                {
                    Encoding = Encoding.UTF8
                };
                var result = await downloader.DownloadStringTaskAsync(new Uri(url));

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            // return null;

        }

        /// <summary>
        /// Прежде всего вызывается проверка на соединение
        /// отсутствует - значит нет комментариев
        /// </summary>
        /// <returns></returns>
        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (var stream = client.OpenRead("http://www.google.com"))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
    }
}