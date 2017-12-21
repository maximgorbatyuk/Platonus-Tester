using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Win32;
using Platonus_Tester.Controller;
using Platonus_Tester.Helper;
using Platest.Controllers;
using Platest.Interfaces;
using Platest.Models;
using ResultComments.Models;

namespace Platonus_Tester
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ISourceLoadListener
    {

        private IQuestionManager _questionManager;
        private TestQuestion        _currentQuestion;
        private List<AnsweredQuestion> _answered;
        private int _count;
        private bool _loadedFile;
        private string _fileName = "";
        private SourceController _sourceController;
        //-------------------------
        private Settings _settings;
        private Comment _goodComment, _badComment;
        private SourceFile _sourcefile = null;

        private readonly List<RadioButton> _radioButtonsList;
        private readonly List<Rectangle> _recList;
        private readonly List<TextBlock> _tbList;


        public MainWindow()
        {

            InitializeComponent();
            Title = Const.ApplicationName;
            WelcomeTextBlock.Text = Const.WelcomeText;
            DescrTextBlock.Text = Const.DescriptionText;
            serviceTextBox.Text = Const.InviteToLoadFile;
            InformationLabel.Content = "";
            VersionLabel.Content = $"Версия: {Const.Version}";
            SettingsButton.Content = Const.SettingsText;
            _radioButtonsList = new List<RadioButton>
            {
                RbVariant1,
                RBVariant2,
                RBVariant3,
                RBVariant4,
                RBVariant5,
            };
            RbVariant1.IsChecked = true;
            _recList = new List<Rectangle>
            {
                Rc1,
                Rc2,
                Rc3,
                Rc4,
                Rc5,
            };

            _tbList = new List<TextBlock>
            {
                V1TextBlock,
                V2TextBlock,
                V3TextBlock,
                V4TextBlock,
                V5TextBlock,
            };

        }

        private void ChangeColorSchemeClick(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();

            //var resource = (Style)FindResource(value ? "LigthWindowStyle" : "DarkWindowStyle");
            //MainWindow1.Style = resource;
            //StartGrid.Style = resource;
            //QuestionGrid.Style = resource;
            //value = !value;
        }

        /// <summary>
        /// Обработка файла, который "скинули" в форму
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartGrid_OnDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

            var files = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (files == null)
            {
                return;
            }

            if (files.Length <= 0) return;
            OpenFile(files[0]);
        }

        private void StartGrid_OnDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;
            e.Handled = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _questionManager    = new QuestionManager();
           // _settings           = SettingsController.Load();
            _sourceController   = new SourceController(this);

            _goodComment = new Comment();
            _badComment = new Swear();
            //---
            // Point renderedLocation = StartGrid.TranslatePoint(new Point(0, 0), MainWindow1);
            StartGrid.TranslatePoint(new Point(0, 0), MainWindow1);
            StartButton.Content = Const.LoadSourceFile;
            _settings = SettingsController.Load();
            SwearLabel.Content = _settings.ShowSwearing ? Const.SwearsEnabled : Const.SwearsDisabled;
            LimitLabel.Content = _settings.EnableLimit ? Const.LimitEnabled : Const.LimitDisabled;
        }

        /// <summary>
        /// Обработка исходоного файла. Загрузка в лейблы.
        /// </summary>
        /// <param name="source"></param>
        private async void ProcessSourceFile(SourceFile source)
        {
            _questionManager.SetSourceList(source);
            if (_settings.EnableLimit)
            {
                _questionManager.SetQuestionLimit(_settings.QuestionLimitCount);
            }
            //----------------------------
            _currentQuestion = _questionManager.GetNext();
            _count += 1;
            LoadToLabels(_currentQuestion);
            var remain = !_settings.EnableLimit
                ? _questionManager.GetCount() - _answered.Count
                : _settings.QuestionLimitCount - _answered.Count;

            InformationLabel.Content = $"Осталось {remain} вопр.";
            LimitLabel.Content = _settings.EnableLimit ? Const.LimitEnabled : Const.LimitDisabled;
            SwearLabel.Content = _settings.ShowSwearing ? Const.SwearsEnabled : Const.SwearsDisabled;
            //
            var count = _questionManager.GetCount();
            if (count > 0)
            {
                serviceTextBox.Text = $"Файл {source.FileName} загружен. Нажмите \"Начать\". Вопросов {count}";
                _loadedFile = true;
                StartButton.IsEnabled = true;
            }
            else
            {
                serviceTextBox.Text = $"Возникли проблемы с обработкой вопросов";
            }
            //--------------------------
            NextButton.Content = Const.NextQuestion;
            CheckButton.Content = Const.CheckQuestion;
            //UInterfaceHelper.SetProgressValue(progressBar, 100);

            var errors = _questionManager.GetErrors();
            if (errors != null)
            {
                new ErrorWindow(errors.ToList()).ShowDialog();
            }
        }

        private void LoadToLabels(Question test)
        {
            if (test == null) return;
            questionTextBlock.Text = test.AskQuestion;

            if (test.Picture != null)
            {
                image1.Source = UInterfaceHelper.GetImageSource(test.Picture);
            }
            image1.Visibility = test.Picture != null ? Visibility.Visible : Visibility.Hidden;

            var hash = new List<string>(0);
            hash.AddRange(test.AnswerList);
            //-----------------
            for (var i = 0; i < _radioButtonsList.Count; i++)
            {
                var rb = _radioButtonsList[i];
                var rec = _recList[i];
                rb.Background = new SolidColorBrush(Const.LigthBackgroundColor);
                rec.Fill = new SolidColorBrush(Const.LigthBackgroundColor );
                //rec.Fill = new SolidColorBrush(Const.LigthBackgroundColor );

                _tbList[i].Text = GetRandomItem(hash, i);
                // rb.Content = GetRandomItem(hash, i);
                // hash.Remove((string) rb.Content);
                hash.Remove(_tbList[i].Text);
            }
            //---------------------------
        }

        private static string GetRandomItem(IList<string> hash, int i)
        {
            var random = new Random(DateTime.Now.Millisecond + i);
            return hash[random.Next(hash.Count)];
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (_loadedFile)
            {
                StartGrid.Visibility = Visibility.Hidden;
            }
            else
            {
                OpenFile();
            }
        }

        private void LoadSettings()
        {
            serviceTextBox.Background = new SolidColorBrush( Const.LigthBackgroundColor );
            _settings = SettingsController.Load();
            _answered = new List<AnsweredQuestion>(0);
            StartGrid.Visibility = Visibility.Visible;
            if (_fileName == "") return;

            StartButton.IsEnabled = false;
            progressBar.Value = 0;
            _sourceController.ProcessSourceFileAsync(_fileName);
            LimitLabel.Content = _settings.EnableLimit ? Const.LimitEnabled : Const.LimitDisabled;
            SwearLabel.Content = _settings.ShowSwearing ? Const.SwearsEnabled : Const.SwearsDisabled;
        }

        /// <summary>
        /// Создание диалогового окна выбора файла и вызов обработки, если файл соответсвует валидации
        /// </summary>
        /// <param name="dragname"></param>
        private void OpenFile(string dragname = null)
        {
            serviceTextBox.Background = new SolidColorBrush( Const.LigthBackgroundColor );
            if (dragname == null)
            {
                var openFileDialog1 = new OpenFileDialog
                {
                    //InitialDirectory = Directory.GetCurrentDirectory(),
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    FilterIndex = 2,
                    RestoreDirectory = true
                };
                //--------------------------
                var result = openFileDialog1.ShowDialog();
                if (!result ?? false) return;
                _fileName = openFileDialog1.FileName;
            }
            else
            {
                _fileName = dragname;
            }
            if (!ValidateFilename(_fileName))
            {
                serviceTextBox.Text = Const.WrongFilename;
                return;
            }
            serviceTextBox.Text = Const.FileProcessing;
            progressBar.IsIndeterminate = true;
            progressBar.Visibility = Visibility.Visible;
            //UInterfaceHelper.SetText(serviceTextBox, Const.FileProcessing);
            LoadSettings();
        }
        
        /// <summary>
        /// Валидация имени файла. Пока что только проверка на расширение файла
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private static bool ValidateFilename(string file)
        {
            return file.IndexOf(".docx", StringComparison.Ordinal) > -1 ||
                   file.IndexOf(".doc", StringComparison.Ordinal) > -1 ||
                   file.IndexOf(".txt", StringComparison.Ordinal) > -1;
        }

        private void StartGrid_DragEnter(object sender, DragEventArgs e)
        {
            serviceTextBox.Text = Const.DraggingFiles;
            serviceTextBox.Background = new SolidColorBrush( Const.LigthBlack );
            e.Effects = e.Data.GetDataPresent(DataFormats.FileDrop) ?
                DragDropEffects.Move :
                DragDropEffects.None;
        }

        private void MainWindow1_Initialized(object sender, EventArgs e)
        {
            // StartGrid
        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(
                "https://github.com/maximgorbatyuk/Platonus-Tester/#platonus-tester");
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            var settingsView = new SettingsForm();
            settingsView.Closed += SettingsViewOnClosed;
            settingsView.ShowDialog();            
        }

        private void SettingsViewOnClosed(object sender, EventArgs eventArgs)
        {
            _settings = SettingsController.Load();
            LimitLabel.Content = _settings.EnableLimit ? Const.LimitEnabled : Const.LimitDisabled;
            SwearLabel.Content = _settings.ShowSwearing ? Const.SwearsEnabled : Const.SwearsDisabled;
        }

        /// <summary>
        /// Функция считывает тайтл варианта ответа, который был отмечен, и записывает его в новый объект - 
        /// отвеченный вопрос, добавляя к массиву отвеченных
        /// </summary>
        private void CheckQuestion()
        {
            if (_currentQuestion == null) return;
            var answer = new AnsweredQuestion
            {
                AskQuestion = _currentQuestion.AskQuestion,
                CorrectAnswer = _currentQuestion.CorrectAnswer
            };
            //------------------------
            foreach (var rb in _radioButtonsList)
            {
                var tb = (TextBlock) rb.Content;
                if (rb.IsChecked != null && rb.IsChecked.Value)
                {
                    answer.ChosenAnswer = tb.Text;
                }
            }
            answer.IsItCorrect = answer.ChosenAnswer == answer.CorrectAnswer;
            _answered.Add(answer);
        }

        private void FinishTesting()
        {
            if ((_answered != null) && (_answered.Count > 0))
            {
                new ResultWindow(_answered, _goodComment, _badComment).ShowDialog();
            }
            StartGrid.Visibility = Visibility.Visible;
            _loadedFile = false;
            serviceTextBox.Text = Const.InviteToLoadFile;
            StartButton.Content = Const.LoadSourceFile;
            InformationLabel.Content = "";
        }

        private void SettingsMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            SettingsButton_Click(sender, e);
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentQuestion == null) return;

            for (var index = 0; index < _radioButtonsList.Count; index++)
            {
                var rb = _radioButtonsList[index];
                var tb = (TextBlock)rb.Content;
                if (tb.Text == _currentQuestion.CorrectAnswer)
                {
                    UInterfaceHelper.PaintBackColor(_recList[index], true);
                    
                    //tb.Background = new SolidColorBrush(Const.CorrectColor);
                    //_tbList[index].Background = new SolidColorBrush(Const.CorrectColor);
                }
                else if (rb.IsChecked ?? false)
                {
                    UInterfaceHelper.PaintBackColor(_recList[index], false);
                    //tb.Background = new SolidColorBrush(Const.IncorrectColor);
                }
            }
        }

        /// <summary>
        /// Отобрадение прогресса отвеченных вопрсов в отношении всего массива
        /// Just for Fun
        /// </summary>
        /// <param name="position"></param>
        private void DisplayProgress(int position)
        {
            var count = _settings.EnableLimit ? _settings.QuestionLimitCount : _questionManager.GetCount();
            count = count > 0 ? count: 1;
            var pValue = (double)position / count * 100;
            pValue = pValue > 100 ? 100 : pValue;
            progressBar.Value = pValue;
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            var cond = _answered.Count == _questionManager.GetFirstListCount() ||
                       (_answered.Count == _settings.QuestionLimitCount && _settings.EnableLimit == true);
            if (cond)
            {
                //_settings = SettingsController.Load();
                //swearLabel.Content = _settings.ShowSwearing ? Const.SwearsEnabled : Const.SwearsDisabled;
                FinishTesting();
            }
            else
            {
                CheckQuestion();
                _currentQuestion = _questionManager.GetNext();
                LoadToLabels(_currentQuestion);
                DisplayProgress(_questionManager.GetCurrentPosition());

                var remain = !_settings.EnableLimit ? 
                    _questionManager.GetCount() - _answered.Count : 
                    _settings.QuestionLimitCount - _answered.Count;

                InformationLabel.Content = $"Осталось вопросов: {remain}";

                if (remain == 0)
                {
                    NextButton.Content = Const.ShowResult;
                }
            }
        }

        private void StartAgainMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (_fileName == "") return;
            UInterfaceHelper.SetText(InformationLabel, "");
            UInterfaceHelper.SetText(serviceTextBox, Const.FileProcessing);
            LoadSettings();
            // throw new NotImplementedException();
        }

        private void LoadSourceMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFile();
        }

        private void FinishMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            FinishTesting();
        }

        public void OnSourceLoaded(SourceFile file)
        {
            _sourcefile = file;
            if (_sourcefile?.SourceText == null)
            {
                serviceTextBox.Text = Const.InviteToLoadFile;
                StartButton.IsEnabled = true;
                //StartButton.Content = "Поместите файл в окно";
                return;
            }
            StartButton.Content = Const.StartTesting;
            progressBar.IsIndeterminate = false;
            progressBar.Value = 0;
            ProcessSourceFile(_sourcefile);
        }
    }
}
