using System;
using Caliburn.Micro;

namespace Coin.Shared
{
    public static class Dialog
    {
        public static DialogViewModel WithContent(IScreen content)
        {
            return new DialogViewModel().WithContent(content);
        }
    }

    public class DialogViewModel : Screen
    {
        private IScreen _content;
        private bool _hasAcceptButton;
        private bool _hasCancelButton;

        public IScreen Content
        {
            get { return _content; }
            private set
            {
                if (Equals(value, _content)) return;
                _content = value;
                NotifyOfPropertyChange(() => Content);
            }
        }

        public bool HasAcceptButton
        {
            get { return _hasAcceptButton; }
            private set
            {
                if (value == _hasAcceptButton) return;
                _hasAcceptButton = value;
                NotifyOfPropertyChange(() => HasAcceptButton);
            }
        }

        public bool HasCancelButton
        {
            get { return _hasCancelButton; }
            private set
            {
                if (value == _hasCancelButton) return;
                _hasCancelButton = value;
                NotifyOfPropertyChange(() => HasCancelButton);
            }
        }

        public bool? DialogResult { get; private set; }

        public DialogViewModel()
        {
            HasAcceptButton = true;
            HasCancelButton = true;
        }

        public DialogViewModel WithContent(IScreen content)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));

            Content = content;
            Content.ConductWith(this);
            return this;
        }

        public DialogViewModel WithAcceptButton()
        {
            HasAcceptButton = true;
            return this;
        }

        public DialogViewModel WithCancelButton()
        {
            HasCancelButton = true;
            return this;
        }

        public void Accept()
        {
            DialogResult = true;
            TryClose(DialogResult);
        }

        public void Cancel()
        {
            DialogResult = null;
            TryClose(DialogResult);
        }
    }
}