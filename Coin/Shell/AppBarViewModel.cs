using System;
using Caliburn.Micro;
using Coin.Shared;

namespace Coin.Shell
{
    public class AppBarViewModel : PropertyChangedBase
    {
        private readonly IEventAggregator _events;

        public AppBarViewModel(IEventAggregator events)
        {
            if (events == null) throw new ArgumentNullException(nameof(events));
            _events = events;
        }

        public void RefreshData()
        {
            _events.PublishOnUIThread(new RefreshRequested());
        }
    }
}