using System.Drawing;
using System.Windows.Controls;
using System.Windows.Media;
using Platonus_Tester.Model;

namespace Platonus_Tester.Helper
{
    public class TextBlockFactory
    {
        private readonly Font _font;
        private readonly int _labelWidth;

        public TextBlockFactory(Font font, int width)
        {
            _labelWidth = width;
            _font = font;
        }

        public TextBlock GetAnsweredLabel(AnsweredQuestion q, int count, Point point)
        {
            var label = GetLabelTemplate(point);
            label.Text =
                q.IsItCorrect ?
                $"Вопрос №{count}\nВопрос: {q.AskQuestion}\nПравильный ответ: {q.ChosenAnswer}\nОтвечен верно" :
                $"Вопрос №{count}\nВопрос: {q.AskQuestion}\nПравильный ответ: {q.CorrectAnswer}\nОтвет пользователя: {q.ChosenAnswer}";

            label.Background = new SolidColorBrush(
                q.IsItCorrect ?
                Const.CorrectColor :
                Const.IncorrectColor
                );
            return label;
        }

        public TextBlock GetErrorLabel(string text, Point point)
        {
            var label = GetLabelTemplate(point);
            label.Text = text;
            label.Background = new SolidColorBrush( Const.IncorrectColor );
            return label;
        }



        private TextBlock GetLabelTemplate(Point point)
        {
            /*
            var label = new TextBlock
            {
                Location = point,
                Font = _font,
                AutoSize = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Height = 115,
                Width = _labelWidth
            };

            return label;
            */
            return null;
        }
    }
}