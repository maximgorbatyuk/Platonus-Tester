using System;
using System.Reflection;
using System.Windows.Media;

namespace Platonus_Tester.Helper
{
    public class Const
    {
        public static readonly string ApplicationName = "Platonus Tester";
        public static readonly string DescriptionText =
            "Краткое описание: \n " +
            "Программа работает с файлами формата*.txt, *.doc и *.docx. " +
            "Достаточно просто перетянуть файл в форму или выбрать в диалоговом окне\n\n" +
            "Более подробно можете узнать в описании программы, доступном на ГитХабе разработчика." +
            "Кнопка 'О программе' как раз перенаправит пользователя на сайт с описанием.\n\n" +
            "В программе присутствут ругательства! Если вы не желаете их видеть, то настройте вывод ругательств в настройках." +
            "По умолчанию они выключены";

        public static readonly string Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public static readonly string WelcomeText = "Добро пожаловать в программу тестирования";
        public static readonly string LoadSourceFile = "Загрузить тест";
        public static readonly string StartTesting = "Начать тестирование";
        public static readonly string CheckQuestion = "Проверить тест";
        public static readonly string ProgrammSetings = "Настройки программы";
        public static readonly string DraggingFiles = "Захватываю файл";
        public static readonly string FileProcessing = "Загружается файл. Это может занять некоторое время";
        public static readonly string ShowResult = "Показать результат";
        public static readonly string SwearsEnabled = "Включено отображение ругательств. Вы сами этого хотите";
        public static readonly string SwearsDisabled = "Ругательства выключены";
        public static readonly string NextQuestion = "Следующий вопрос";
        public static readonly string WrongFilename = "Неверный формат файла";
        public static readonly string MissingVariant = "Ошибка: отсутствует вариант ответа";
        public static readonly string CheckThis = "Проверить";
        public static readonly string ProcessingProblem = $"Возникли проблемы с обработкой вопросов";
        public static readonly string ResultTitle = "Результаты тестирования";

        public static string TEST_URL = "https://raw.githubusercontent.com/maximgorbatyuk/Test-Unit-Project/master/SessionTestUnit/HashSource/test.txt";
        public static string HASH_100_URL = "https://raw.githubusercontent.com/maximgorbatyuk/Test-Unit-Project/master/SessionTestUnit/HashSource/hash_100.txt";
        public static string HASH_90_URL = "https://raw.githubusercontent.com/maximgorbatyuk/Test-Unit-Project/master/SessionTestUnit/HashSource/hash_90.txt";
        public static string HASH_75_URL = "https://raw.githubusercontent.com/maximgorbatyuk/Test-Unit-Project/master/SessionTestUnit/HashSource/hash_75.txt";
        public static string HASH_60_URL = "https://raw.githubusercontent.com/maximgorbatyuk/Test-Unit-Project/master/SessionTestUnit/HashSource/hash_60.txt";
        public static string HASH_50_URL = "https://raw.githubusercontent.com/maximgorbatyuk/Test-Unit-Project/master/SessionTestUnit/HashSource/hash_50.txt";
        public static string HASH_0_URL = "https://raw.githubusercontent.com/maximgorbatyuk/Test-Unit-Project/master/SessionTestUnit/HashSource/hash_0.txt";


        public static readonly Color LigthBlack = Color.FromArgb(118, 0, 0, 0);
        public static readonly Color LigthBackgroundColor = Color.FromArgb(255, 240, 240, 240);
        public static readonly Color DarkBackgroundColor = Color.FromArgb(255, 43, 43, 43);

        public static readonly Color LigthFontColor = Color.FromArgb(255, 33, 33, 33);
        public static readonly Color DarkhFontColor = Color.FromArgb(255, 228, 228, 228);

        public static readonly Color CorrectColor = Color.FromArgb(255, 144, 232, 121);
        public static readonly Color IncorrectColor = Color.FromArgb(255, 233, 107, 107);

        public static readonly string InviteToLoadFile  = "Перетяните файл в окно";
        public static readonly string PickAnAnswer      = "Выберите вопрос для просмотра";
    }
}