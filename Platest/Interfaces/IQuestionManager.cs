using System.Collections.Generic;
using Platest.Models;

namespace Platest.Interfaces
{
    public interface IQuestionManager
    {
        void SetSourceList(SourceFile file);

        void Shuffle();

        TestQuestion GetNext();

        int GetCount();

        int GetFirstListCount();

        void SetQuestionLimit(int limit);

        int GetCurrentPosition();

        IEnumerable<string> GetErrors();
    }
}