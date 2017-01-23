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
            }
        }

        public DateTime? GetSelectedDateTime()
        {
            return SelectedDate?.Add(SelectedTime ?? TimeSpan.Zero);
        }
    }
}