using Platonus_Tester.Model;

namespace Platonus_Tester.CustomArgs
{
    /// <summary>
    /// Кастомный аргумент-класс, инициализирующийся при окончании обработки файла
    /// </summary>
    public class SourceFileLoadedArgs
    {
        public SourceFile ProcessingResult;

        public SourceFileLoadedArgs(SourceFile result)
        {
            ProcessingResult = result;
        }
    }
}