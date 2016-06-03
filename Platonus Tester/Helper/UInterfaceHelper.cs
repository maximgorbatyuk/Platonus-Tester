using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Image = System.Windows.Controls.Image;

namespace Platonus_Tester.Helper
{
    public abstract class UInterfaceHelper
    {
        public static void SetWidth(Control label, int width)
        {
            label.Dispatcher.Invoke(new Action<int>(w => label.Width = w), width);
        }


        public static void SetText(ContentControl control, string text)
        {
            control.Dispatcher.Invoke(new Action<string>(s => control.Content = s), text);
        }

        public static void SetEnable(Control control, bool state)
        {
            control.Dispatcher.Invoke(new Action<bool>(s => control.IsEnabled = s), state);
        }

        public static void SetVisible(ContentControl control, bool state)
        {
            var visibiliy = state ? Visibility.Visible : Visibility.Hidden;
            control.Dispatcher.Invoke(
                new Action<bool>(s => control.Visibility = visibiliy), visibiliy);
        }

        public static void SetVisible(Image control, bool state)
        {
            var visibiliy = state ? Visibility.Visible : Visibility.Hidden;
            control.Dispatcher.Invoke(
                new Action<Visibility>(s => control.Visibility = s), visibiliy);
        }

        public static void SetImage(Image control, System.Drawing.Image image)
        {
            ImageSource source = null;
            using (var memory = new MemoryStream())
            {
                image.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                var bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                source = bitmapimage;
            }
            if (source == null)
            {
                return;
            }
            control.Dispatcher.Invoke(
                    new Action<ImageSource>(s =>
                    {
                        try
                        {
                            control.Source = s;
                        }
                        catch (InvalidOperationException ex)
                        {
                            throw ex;
                        }
                    }), source);
            
        }


        public static void SetProgressValue(ProgressBar control, int value)
        {
            control.Dispatcher.Invoke(
                new Action<int>(s => control.Value = s), value);
        }

        public static void PaintBackColor(ContentControl sender, bool rigth)
        {
            sender.Background = new SolidColorBrush(rigth ? Const.CorrectColor : Const.IncorrectColor );
        }

        internal static void SetText(TextBlock serviceTextBox, string text)
        {
            serviceTextBox.Dispatcher.Invoke(
                new Action<string>(s => serviceTextBox.Text = s), text);
        }
    }
}