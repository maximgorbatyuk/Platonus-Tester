namespace Platest.Models
{
    /// <summary>
    /// тестовый (незаданный) вопрос. Добавляется только поле Верный ответ
    /// </summary>
    public class TestQuestion : Question
    {
        public string CorrectAnswer { get; set; }
    }
}