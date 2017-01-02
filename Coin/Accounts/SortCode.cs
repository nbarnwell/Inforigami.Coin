namespace Coin.Accounts
{
    public class SortCode
    {
        public string Value { get; private set; }

        public SortCode(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}