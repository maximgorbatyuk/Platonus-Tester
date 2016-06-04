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
using Platonus_Tester.Controller;
using Platonus_Tester.Helper;

namespace Platonus_Tester
{
    /// <summary>
    /// Логика взаимодействия для SettingsForm.xaml
    /// </summary>
    public partial class SettingsForm : Window
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetLimitCountTextBlock.Text = Const.LimitCountSet;
            LimitEnableTextBlock.Text = Const.LimitEnableText;
            ShowSwearsEnabledTextBlock.Text = Const.ShowSwearEnableText;
            DownloadSwearsTextBlock.Text = Const.DownloadSwearEnableText;
            ColorSchemeTextBlock.Text = Const.ColorChemeEnabledText;
            //----------------------
            var settings = SettingsController.Load();
            LimitEnableCheckBox.IsChecked = settings.EnableLimit;
            LimitEnableCheckBox.Content = settings.EnableLimit ? Const.Enabled : Const.Disabled;

            LimitCountTextBox.Text = $"{settings.QuestionLimitCount}";

            ShowSwearsCheckBox.IsChecked = settings.ShowSwearing;
            ShowSwearsCheckBox.Content = settings.ShowSwearing ? Const.Enabled : Const.Disabled;

            DownloadSwearsCheckBox.IsChecked = settings.DownloadSwears;
            DownloadSwearsCheckBox.Content = settings.DownloadSwears ? Const.Enabled : Const.Disabled;

            ColorSchemeCheckBox.IsChecked = settings.LightColorScheme;
            ColorSchemeCheckBox.Content = settings.LightColorScheme ? Const.Enabled : Const.Disabled;
        }

        private void SaveSettings()
        {
            var limitCount = 25;
            try
            {
                limitCount = int.Parse(LimitCountTextBox.Text);
            }
            catch (Exception ex)
            {
                
            }
            var settings = new Settings
            {
                EnableLimit = LimitEnableCheckBox.IsEnabled,
                ShowSwearing = ShowSwearsCheckBox.IsEnabled,
                DownloadSwears = DownloadSwearsCheckBox.IsEnabled,
                LightColorScheme = ColorSchemeCheckBox.IsEnabled,
                QuestionLimitCount = limitCount
            };
            SettingsController.SaveSettings(settings);
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            SaveSettings();
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            var cb = (CheckBox) sender;
            try
            {
                cb.Content = cb.IsChecked.Value ? Const.Enabled : Const.Disabled;
            }
            catch
            {
                // ignored
            }
        }
    }
}
