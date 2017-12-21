namespace Platest.Models
{
    /// <summary>
    /// Отвеченный вопрос, где добавляемые поля - отмеченный, верный и флаг корректности (лол)
    /// </summary>
    public class AnsweredQuestion : Question
    {
        public string ChosenAnswer;
        public string CorrectAnswer;
        public bool IsItCorrect;

        public override string ToString()
        {
            var result = $"{AskQuestion}";
            return result;
        }
    }

   
}