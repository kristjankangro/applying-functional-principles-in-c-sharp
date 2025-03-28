namespace ErrorsAndFailures
{
    public class Customer
    {
        private decimal Balance { get; set; }
        public string BillingInfo { get; private set; }

        public void AddBalance(MoneyToCharge amount)
        {
            Balance += amount;
        }
    }
}
