using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using Platest.Models;
using Platonus_Tester.Controller;
using Platonus_Tester.Helper;
using ResultComments.Models;

namespace Platonus_Tester
{
    /// <summary>
    /// Логика взаимодействия для ResultWindow.xaml
    /// </summary>
    public partial class ResultWindow : Window
    {

        private int _rigth;
        private readonly List<AnsweredQuestion> _hash;
        private readonly Swear _swearHelper;
        private readonly Comment _goodHelper;
        private readonly Settings _settings;

        public ResultWindow(List<AnsweredQuestion> hash, Comment good, Comment bad)
        {
            InitializeComponent();
            _hash = hash;
            _goodHelper = good;
            _swearHelper = bad as Swear;
            _settings = SettingsController.Load();
            _rigth = 0;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Title = Const.ResultTitle;
            answerTextBlock.Text = Const.PickAnAnswer;
            foreach (var a in _hash)
            {
                if (a.IsItCorrect)
                {
                    _rigth++;
                }
            }
            TextBlock_AnswerCount.Text = $"Всего вопросов: {_hash.Count}. Отмечено верно: {_rigth}";
            var result = (double)_rigth / _hash.Count;
            result = result * 100;
            commentTextBlock.Text = $"Ваш результат: {result:#.##}%\n{GetComment(result)}";
            LoadListBox(_hash);
        }

        private string GetComment(double res)
        {
            return _settings.ShowSwearing ? _swearHelper.Get(res) : _goodHelper.Get(res);
        }

        private void LoadListBox(IEnumerable<AnsweredQuestion> hash)
        {
            listBox.Items.Clear();
            foreach (var q in hash)
            {
                var text = q.IsItCorrect ? $"Верно: {q}" : $"Неверно: {q}";
                listBox.Items.Add(text);
                //var item = (ListBoxItem)listBox.Items.
                //item.Style = (Style)FindResource(q.IsItCorrect ? "CorrectAnswerStyle" : "IncorrectAnswerStyle");
            }
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var index = listBox.SelectedIndex;
            var item = _hash[index];
            answerTextBlock.Text = $"{item.AskQuestion}\nПравильный ответ: {item.CorrectAnswer}";
            answerTextBlock.Text += !item.IsItCorrect ? $"\nОтвет юзера: {item.ChosenAnswer}" : "";
            answerTextBlock.Background = new SolidColorBrush(
                item.IsItCorrect ? Const.CorrectColor : Const.IncorrectColor);
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
