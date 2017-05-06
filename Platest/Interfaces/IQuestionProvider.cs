using System.Collections.Generic;
using Platest.Models;

namespace Platest.Interfaces
{
    public interface IQuestionProvider
    {
        List<TestQuestion> GetQuestionList(SourceFile file);

        IEnumerable<string> GetErrors();
    }
}