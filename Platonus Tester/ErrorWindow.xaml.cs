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

namespace Platonus_Tester
{
    /// <summary>
    /// Логика взаимодействия для ErrorWindow.xaml
    /// </summary>
    public partial class ErrorWindow : Window
    {

        private readonly List<string> _errorList;

        public ErrorWindow(List<string> source)
        {
            InitializeComponent();
            _errorList = source;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            listBox.Items.Clear();

            foreach (var error in _errorList)
            {
                listBox.Items.Add(error);
            }
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (string) listBox.SelectedItem;
            textBlock.Text = item;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
