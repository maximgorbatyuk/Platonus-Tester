namespace Platonus_Tester.Helper
{
    /// <summary>
    /// настройки программы, записывающиеся в XML формате
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Установить лимит вопросов (25 обычно), как на тестировании
        /// </summary>
        public bool EnableLimit { get; set; }
        /// <summary>
        /// Показать ругательные комментарии
        /// </summary>
        public bool ShowSwearing { get; set; }
        /// <summary>
        /// Загружать ли комментарии с репозитория
        /// </summary>
        public bool DownloadSwears { get; set; }
        /// <summary>
        /// Будет добавлена в будущем поддержка двух цветовых схем.
        /// </summary>
        public bool LightColorScheme { get; set; }
        /// <summary>
        /// Количество лимита 
        /// </summary>
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