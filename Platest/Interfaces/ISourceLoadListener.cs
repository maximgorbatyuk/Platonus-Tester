using Platest.Models;

namespace Platest.Interfaces
{
    public interface ISourceLoadListener
    {
        void OnSourceLoaded(SourceFile file);
    }
}