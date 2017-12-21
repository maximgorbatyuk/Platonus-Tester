﻿using System;
using System.Reflection;
using System.Windows.Media;

namespace Platonus_Tester.Helper
{
    /// <summary>
    /// Here I tryed to make an imitation of String resource file
    /// This variables a assigned to Control-components contents and titles
    /// Also here Color const placed.
    /// </summary>
    public class Const
    {
        public static readonly string ApplicationName = "Platonus Tester";
        public static readonly string DescriptionText =
            "Краткое описание: \n " +
            "Программа работает с файлами формата*.txt, *.doc и *.docx. " +
            "Достаточно просто перетянуть файл в форму или выбрать в диалоговом окне\n\n" +
            "Более подробно можете узнать в описании программы, доступном на ГитХабе разработчика. " +
            "Кнопка 'О программе' как раз перенаправит пользователя на сайт с описанием.\n\n" +
            "В программе присутствут ругательства! Если вы не желаете их видеть, то настройте вывод ругательств в настройках. " +
            "По умолчанию они выключены";

        public static readonly string Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public static readonly string WelcomeText = "Добро пожаловать в программу тестирования";
        public static readonly string LoadSourceFile = "Загрузить тест";
        public static readonly string StartTesting = "Начать тест";
        public static readonly string CheckQuestion = "Проверить вопрос";
        public static readonly string ProgrammSetings = "Настройки программы";
        public static readonly string DraggingFiles = "Захватываю файл";
        public static readonly string FileProcessing = "Загружается файл. Это может занять некоторое время";
        public static readonly string ShowResult = "Показать результат";
        public static readonly string SwearsEnabled = "Ругательства включены. Так желает юзер";
        public static readonly string SwearsDisabled = "Ругательства выключены";
        public static readonly string NextQuestion = "Следующий вопрос";
        public static readonly string WrongFilename = "Неверный формат файла";
        // public static readonly string MissingVariant = "Ошибка: отсутствует вариант ответа";
        public static readonly string CheckThis = "Проверить";
        public static readonly string ProcessingProblem = $"Возникли проблемы с обработкой вопросов";
        public static readonly string ResultTitle = "Результаты тестирования";

        public static readonly string LimitEnabled = "Лимит включен";
        public static readonly string LimitDisabled = "Все вопросы";
        
        public static readonly Color LigthBlack = Color.FromArgb(118, 0, 0, 0);
        public static readonly Color LigthBackgroundColor = Color.FromArgb(255, 245, 245, 245);
        public static readonly Color DarkBackgroundColor = Color.FromArgb(255, 43, 43, 43);

        public static readonly Color LigthFontColor = Color.FromArgb(255, 33, 33, 33);
        public static readonly Color DarkhFontColor = Color.FromArgb(255, 228, 228, 228);

        public static readonly Color CorrectColor = Color.FromArgb(255, 144, 232, 121);
        public static readonly Color IncorrectColor = Color.FromArgb(255, 233, 107, 107);

        public static readonly string InviteToLoadFile  = "Поместите файл в окно";
        public static readonly string PickAnAnswer      = "Выберите вопрос для просмотра";

        public static readonly string Enabled = "Вкл";
        public static readonly string Disabled = "Выкл";
        public static readonly string LimitEnableText = "Установить лимит вопросов (не все вопросы будут выведены)";
        public static readonly string LimitCountSet = "Установить число лимита вопросов";
        public static readonly string ShowSwearEnableText = "Показывать ругательства ;) в комментариях";
        public static readonly string DownloadSwearEnableText = "Загружать ругательства ;) из GitHub";
        public static readonly string ColorChemeEnabledText = "Светлая цветовая схема";
        public static readonly string SettingsText = "Настройки";
    }
}