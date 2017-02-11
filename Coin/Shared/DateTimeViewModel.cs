using System;
using Caliburn.Micro;

namespace Coin.Shared
{
    public class DateTimeViewModel : PropertyChangedBase
    {
        private DateTime? _selectedDate;
        private TimeSpan? _selectedTime;

        public DateTime? SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                if (value.Equals(_selectedDate)) return;
                _selectedDate = value;
                NotifyOfPropertyChange(() => SelectedDate);
                NotifyOfPropertyChange(() => SelectedDateTime);
            }
        }

        public TimeSpan? SelectedTime
        {
            get { return _selectedTime; }
            set
            {
                if (value.Equals(_selectedTime)) return;
                _selectedTime = value;
                NotifyOfPropertyChange(() => SelectedTime);
                NotifyOfPropertyChange(() => SelectedDateTime);
            }
        }

        public DateTime SelectedDateTime => SelectedDate.GetValueOrDefault().Add(SelectedTime.GetValueOrDefault());

        public static DateTimeViewModel CreateFrom(DateTime? dateTime, TimeSpan? timeOfDay)
        {
            return new DateTimeViewModel
            {
                SelectedDate = dateTime,
                SelectedTime = timeOfDay
            };
        }

        public DateTimeOffset? GetDateTimeOffset()
        {
            return
                SelectedDate != null
                    ? new DateTimeOffset(SelectedDate.Value.Add(SelectedTime ?? TimeSpan.Zero), DateTimeOffset.Now.Offset)
                    : (DateTimeOffset?) null;
        }
    }
}