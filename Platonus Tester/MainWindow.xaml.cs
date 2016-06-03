using System;
using System.Collections.Generic;
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

namespace Platonus_Tester
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ChangeColorSchemeClick(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            StartGrid.Visibility = StartGrid.IsVisible ? Visibility.Hidden : Visibility.Visible;
        }

        private void StartGrid_OnDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                textBlock1.Text = "";
                var files = (string[]) e.Data.GetData(DataFormats.FileDrop);
                foreach (var file in files)
                {
                    // textBlock1.Text += $"{file} \n";

                }  
            }
            
            
            ;
            // throw new NotImplementedException();
        }

        private void StartGrid_OnDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;
            e.Handled = true;
            // throw new NotImplementedException();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            StartGrid.Visibility = Visibility.Hidden;
        }
    }
}
