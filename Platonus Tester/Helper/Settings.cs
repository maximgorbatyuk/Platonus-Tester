namespace Platonus_Tester.Helper
{
    public class Settings
    {
        public bool EnableLimit { get; set; }
        public bool ShowSwearing { get; set; }
        public bool DownloadSwears { get; set; }
        public bool LightColorScheme { get; set; }
        public int  QuestionLimitCount { get; set; }

        public Settings()
        {
            EnableLimit = false;
            ShowSwearing = false;
            DownloadSwears = false;
            LightColorScheme = true;
            QuestionLimitCount = 25;
        }
    }
}