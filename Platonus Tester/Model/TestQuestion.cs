namespace Platonus_Tester.Model
{
    /// <summary>
    /// тестовый (незаданный) вопрос. Добавляется только поле Верный ответ
    /// </summary>
    public class TestQuestion : Question
    {
        public string CorrectAnswer { get; set; }
    }
}