using Platonus_Tester.Model;

namespace Platonus_Tester.CustomArgs
{
    public class SourceFileLoadedArgs
    {
        public SourceFile ProcessingResult;

        public SourceFileLoadedArgs(SourceFile result)
        {
            ProcessingResult = result;
        }
    }
}