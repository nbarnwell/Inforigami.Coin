using Caliburn.Micro;
using Coin.Shared;

namespace Coin.Playground
{
    public class PlaygroundWorkspace : Conductor<PropertyChangedBase>.Collection.OneActive
    {
        public override string DisplayName => "Playground";

        protected override void OnInitialize()
        {
            ActivateItem(new DateTimeViewModel());
        }
    }
}