namespace Platonus_Tester.Model
{
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