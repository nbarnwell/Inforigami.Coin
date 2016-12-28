using Caliburn.Micro;

namespace Coin.People
{
    public class PersonViewModel : Screen
    {
        private int _personId;
        private string _personName;

        public int PersonId
        {
            get { return _personId; }
            set
            {
                if (value == _personId) return;
                _personId = value;
                NotifyOfPropertyChange(() => PersonId);
            }
        }

        public string PersonName
        {
            get { return _personName; }
            set
            {
                if (value == _personName) return;
                _personName = value;
                NotifyOfPropertyChange(() => PersonName);
            }
        }

        public void UpdateFrom(Data.Person person)
        {
            PersonId = person.Id;
            PersonName = person.Name;
        }

        public static PersonViewModel CreateFrom(Data.Person person)
        {
            return new PersonViewModel {PersonName = person.Name, PersonId = person.Id};
        }
    }
}