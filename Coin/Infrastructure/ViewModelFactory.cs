using Caliburn.Micro;

namespace Coin.Infrastructure
{
    public class ViewModelFactory : IViewModelFactory
    {
        public T Create<T>()
        {
            return IoC.Get<T>();
        }
    }
}