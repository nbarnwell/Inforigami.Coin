namespace Coin.Accounts
{
    public class AccountNumber
    {
        public string Value { get; private set; }

        public AccountNumber(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}