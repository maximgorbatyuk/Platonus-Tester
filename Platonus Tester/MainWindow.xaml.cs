using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
using Microsoft.Win32;
using Platonus_Tester.Comments;
using Platonus_Tester.Controller;
using Platonus_Tester.CustomArgs;
using Platonus_Tester.Helper;
using Platonus_Tester.Model;

namespace Platonus_Tester
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private QuestionController  _questionManager;
        private TestQuestion        _currentQuestion;
        private List<AnsweredQuestion> _answered;
        private int _count;
        private bool _loadedFile;
        private string _fileName = "";
        private SourceController _sourceController;
        //-------------------------
        private Settings _settings;
        private Comment _goodComment, _badComment;

        private readonly List<RadioButton> _radioButtonsList;


        public MainWindow()
        {
            InitializeComponent();
            Title = Const.ApplicationName;
            _radioButtonsList = new List<RadioButton>
            {
                RBVariant1,
                RBVariant2,
                RBVariant3,
                RBVariant4,
                RBVariant5,
            };
        }

        private void ChangeColorSchemeClick(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            StartGrid.Visibility = StartGrid.IsVisible ? Visibility.Hidden : Visibility.Visible;
        }

        private void StartGrid_OnDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;

            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length <= 0) return;
            OpenFile(files[0]);
        }

        private void StartGrid_OnDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;
            e.Handled = true;
            // throw new NotImplementedException();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _questionManager = new QuestionController();

            _sourceController = new SourceController();
            _sourceController.OnLoadComleted += OnSourceLoaded;

            _goodComment = new Comment();
            _badComment = new Swear();
            //---
            // Point renderedLocation = StartGrid.TranslatePoint(new Point(0, 0), MainWindow1);
            StartGrid.TranslatePoint(new Point(0, 0), MainWindow1);
            return;
        }

        private async void ProcessSourceFile(SourceFile sourceFile)
        {
            if (sourceFile.SourceText == null) return;

            _questionManager.SetSourceList(sourceFile);
            if (_settings.QuestionsLimit)
            {
                _questionManager.SetQuestionLimit(25);
            }
            //----------------------------
            _currentQuestion = _questionManager.GetNext();
            _count += 1;
            LoadToLabels(_currentQuestion);
            UInterfaceHelper.SetText(
                informationLabel, $"Осталось вопросов: {_questionManager.GetCount()}");

            UInterfaceHelper.SetText(
                swearLabel, _settings.ShowSwearing ? Const.SwearsEnabled : Const.SwearsDisabled);
            //
            var count = _questionManager.GetCount() + 1;
            if (count > 0)
            {
                UInterfaceHelper.SetText(serviceTextBox, $"Файл загружен. Нажмите \"Начать\". Вопросов {count}");
                _loadedFile = true;
                UInterfaceHelper.SetEnable(StartButton, true);
            }
            else
            {
                UInterfaceHelper.SetText(serviceTextBox,
                    $"Возникли проблемы с обработкой вопросов");
            }
            //--------------------------
            UInterfaceHelper.SetText(NextButton, Const.NextQuestion);
            UInterfaceHelper.SetText(CheckButton, Const.StartTesting);
            UInterfaceHelper.SetProgressValue(progressBar, 100);

            var errors = _questionManager.Errors;
            if (errors != null)
            {
                // new ErrorDisplay(errors).ShowDialog();
            }
            if (_settings.DownloadSwears)
            {
                await StartGettingHashesAsync();
            }
        }

        private void OnSourceLoaded(object sender, SourceFileLoadedArgs e)
        {
            //throw new NotImplementedException();
            var sourceFile = e.ProcessingResult;
            ProcessSourceFile(sourceFile);
        }


        private void LoadToLabels(Question test)
        {
            if (test == null) return;
            UInterfaceHelper.SetText(questionTextBlock, test.AskQuestion);

            if (test.Picture != null)
            {
                UInterfaceHelper.SetImage(image1, test.Picture);
            }
            UInterfaceHelper.SetVisible(image1, test.Picture != null);

            var hash = test.AnswerList;
            //-----------------
            for (var i = 0; i < _radioButtonsList.Count; i++)
            {
                var rb = _radioButtonsList[i];
                rb.Background = new SolidColorBrush(Const.LigthBackgroundColor);
                UInterfaceHelper.SetText(rb, GetRandomItem(hash, i));
                hash.Remove((string) rb.Content);
            }
            //---------------------------
        }

        private static string GetRandomItem(IList<string> hash, int i)
        {
            var random = new Random(DateTime.Now.Millisecond + i);
            return hash[random.Next(hash.Count)];
        }


        private async Task StartGettingHashesAsync()
        {
            foreach (var h in new List<string>
            {
                Const.HASH_100_URL,
                Const.HASH_90_URL,
                Const.HASH_75_URL,
                Const.HASH_60_URL,
                Const.HASH_50_URL,
                Const.HASH_0_URL
            })
            {
                await GetHashListAsync(h);
            }
        }

        private async Task GetHashListAsync(string url)
        {
            // Получение списков ругательств в фоновом процессе
            try
            {
                if (!DownloadController.CheckForInternetConnection()) return;
                var result = await DownloadController.ExecuteRequestAsync(url);
                if (result == null) return;

                var array = SwearHashProcessor.GetHashList(result);
                if (array.Count > 0)
                {
                    _badComment.AddHashList(url, array);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
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

        private async void LoadSettings()
        {
            serviceTextBox.Background = new SolidColorBrush( Const.LigthBackgroundColor );
            _settings = SettingsController.Load();
            _answered = new List<AnsweredQuestion>(0);
            StartGrid.Visibility = Visibility.Visible;
            if (_fileName == "") return;
            swearLabel.Content = _settings.ShowSwearing ? Const.SwearsEnabled : Const.SwearsDisabled;
            UInterfaceHelper.SetEnable(StartButton, false);
            UInterfaceHelper.SetProgressValue(progressBar, 0);
            var source = await _sourceController.ProcessSourceFileAsync(_fileName);
            ProcessSourceFile(source);
        }

        private void OpenFile(string dragname = null)
        {
            serviceTextBox.Background = new SolidColorBrush( Const.LigthBackgroundColor );
            if (dragname == null)
            {
                var openFileDialog1 = new OpenFileDialog
                {
                    InitialDirectory = Directory.GetCurrentDirectory(),
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

            UInterfaceHelper.SetText(serviceTextBox, Const.FileProcessing);
            LoadSettings();
        }

        private bool ValidateFilename(string file)
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
                "https://github.com/maximgorbatyuk/Test-Unit-Project/blob/master/README.md#test-unit-project");
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            new SettingsForm().ShowDialog();
        }


        private void CheckQuestion()
        {
            var answer = new AnsweredQuestion
            {
                AskQuestion = _currentQuestion.AskQuestion,
                CorrectAnswer = _currentQuestion.CorrectAnswer
            };
            //------------------------
            foreach (var rb in _radioButtonsList)
            {
                if (rb.IsChecked ?? false)
                {
                    answer.ChosenAnswer = (string) rb.Content;
                }
            }
            _answered.Add(answer);
        }

        private void FinishTesting()
        {
            if ((_answered != null) && (_answered.Count > 0))
            {
                // new ResultForm(_rigthChecked, _answered, _goodComment, _badComment).ShowDialog();
            }
            StartGrid.Visibility = Visibility.Visible;
            _loadedFile = false;
            serviceTextBox.Text = "";
            StartButton.Content = Const.LoadSourceFile;
        }

        private void SettingsMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            SettingsButton_Click(sender, e);
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentQuestion == null) return;

            foreach (var rb in _radioButtonsList)
            {
                if ((string) rb.Content == _currentQuestion.CorrectAnswer)
                {
                    UInterfaceHelper.PaintBackColor(rb, true);
                }
                else if (rb.IsChecked ?? false)
                {
                    UInterfaceHelper.PaintBackColor(rb, false);
                }
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (_answered.Count == _questionManager.GetFirstListCount())
            {
                _settings = SettingsController.Load();
                swearLabel.Content = _settings.ShowSwearing ? Const.SwearsEnabled : Const.SwearsDisabled;
                FinishTesting();
            }
            else
            {
                CheckQuestion();
                _currentQuestion = _questionManager.GetNext();
                LoadToLabels(_currentQuestion);
                informationLabel.Content = $"Осталось вопросов: {_questionManager.GetCount() - _answered.Count}";
                if (_answered.Count == _questionManager.GetCount())
                {
                    NextButton.Content = Const.ShowResult;
                }
            }
        }

        private void StartAgainMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (_fileName == "") return;
            UInterfaceHelper.SetText(informationLabel, "");
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
    }
}
