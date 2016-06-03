namespace Platonus_Tester.Helper
{
    public class Settings
    {
        public bool QuestionsLimit { get; set; }
        public bool ShowSwearing { get; set; }
        public bool DownloadSwears { get; set; }
        public bool LightColorScheme { get; set; }

        public Settings()
        {
            QuestionsLimit = false;
            ShowSwearing = false;
            DownloadSwears = false;
            LightColorScheme = true;
        }
    }
}