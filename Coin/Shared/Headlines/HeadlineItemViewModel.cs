using Caliburn.Micro;

namespace Coin.Shared.Headlines
{
    public class HeadlineItemViewModel : PropertyChangedBase
    {
        private string _caption;

        public string Caption
        {
            get { return _caption; }
            set
            {
                if (value == _caption) return;
                _caption = value;
                NotifyOfPropertyChange();
            }
        }

        public string Text { get; set; }
    }
}