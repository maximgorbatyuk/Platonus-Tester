using Platest.Models;

namespace Platest.Interfaces
{
    public interface ISourceLoadListener
    {
        void OnSourceLoaded(object currentDispatcherFile);
    }
}