using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Platonus_Tester.Controller
{
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