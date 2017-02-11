using System;
using Caliburn.Micro;
using Inforigami.Regalo.Core;

namespace Coin.Shared
{
    public class EventAggregatorAdapter: IEventHandler<object>
    {
        private readonly IEventAggregator _eventAggregator;

        public EventAggregatorAdapter(IEventAggregator eventAggregator)
        {
            if (eventAggregator == null) throw new ArgumentNullException(nameof(eventAggregator));
            _eventAggregator = eventAggregator;
        }

        public void Handle(object evt)
        {
            _eventAggregator.PublishOnUIThreadAsync(evt);
        }
    }
}