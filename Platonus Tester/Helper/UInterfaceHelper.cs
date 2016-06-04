using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Image = System.Windows.Controls.Image;

namespace Platonus_Tester.Helper
{
    public abstract class UInterfaceHelper
    {
        public static void SetWidth(Control label, int width)
        {
            Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Background,
                new Action(() => label.Width = width));

            //label.Dispatcher.Invoke(DispatcherPriority.Normal,
            //    new Action<int>(w => label.Width = w), width);
        }


        public static void SetText(ContentControl control, string text)
        {
            control.Dispatcher.Invoke(DispatcherPriority.Normal,
                new Action<string>(s => control.Content = s), text);
        }

        public static void SetEnable(Control control, bool state)
        {
            control.Dispatcher.Invoke(DispatcherPriority.Normal,
                new Action<bool>(s => control.IsEnabled = s), state);
        }

        public static void SetVisible(ContentControl control, bool state)
        {
            var visibiliy = state ? Visibility.Visible : Visibility.Hidden;
            control.Dispatcher.Invoke(DispatcherPriority.Normal,
                new Action<Visibility>(s => control.Visibility = s), visibiliy);
        }

        public static void SetVisible(Image control, bool state)
        {
            var visibiliy = state ? Visibility.Visible : Visibility.Hidden;
            control.Dispatcher.Invoke(DispatcherPriority.Normal,
                new Action<Visibility>(s => control.Visibility = s), visibiliy);
        }

        public static ImageSource GetImageSource(System.Drawing.Image s)
        {
            ImageSource result = null;
            using (var memory = new MemoryStream())
            {
                s.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                var bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                result = bitmapimage;
            }
            return result;
        }

        public static void SetImage(Image control, System.Drawing.Image image)
        {
           
            Func<System.Drawing.Image, ImageSource> convert = delegate(System.Drawing.Image s)
            {
                using (var memory = new MemoryStream())
                {
                    s.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                    memory.Position = 0;
                    var bitmapimage = new BitmapImage();
                    bitmapimage.BeginInit();
                    bitmapimage.StreamSource = memory;
                    bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapimage.EndInit();

                    var toReturn = bitmapimage;
                    return toReturn;
                }
            };

            Application.Current.Dispatcher.Invoke(
                DispatcherPriority.Normal,
                new Action(() => control.Source = convert(image) ));
            
        }

        public static string GetContent(ContentControl control)
        {
            Func<ContentControl, string> convert = c => (string) c.Content;
            string result = null;
            Application.Current.Dispatcher.Invoke(
                DispatcherPriority.Normal,
                new Action(() => result = convert(control)));
            return result;
        }


        public static void SetProgressValue(ProgressBar control, int value)
        {
            control.Dispatcher.Invoke(DispatcherPriority.Normal,
                new Action<int>(s => control.Value = s), value);
        }

        public static void PaintBackColor(ContentControl sender, bool rigth)
        {
            sender.Background = new SolidColorBrush(rigth ? Const.CorrectColor : Const.IncorrectColor );
        }

        public static void PaintBackColor(System.Windows.Shapes.Shape sender, bool rigth)
        {
            sender.Fill = new SolidColorBrush(rigth ? Const.CorrectColor : Const.IncorrectColor);
        }

        public static void SetText(TextBlock serviceTextBox, string text)
        {
            //Dispatcher.

            serviceTextBox.Dispatcher.Invoke(DispatcherPriority.Normal,
                new Action<string>(s => serviceTextBox.Text = s), text);
        }

        public static void SetBackground(RadioButton rb, System.Windows.Media.Color ligthBackgroundColor)
        {
            Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Normal,
                new Action(() => rb.Background = new SolidColorBrush(ligthBackgroundColor)));
            // throw new NotImplementedException();
        }
    }
}