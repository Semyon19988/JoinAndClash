using System;

namespace Model.Currency
{
	public class Wallet
	{
		public Wallet(int balance)
		{
			if (balance < 0)
				throw new ArgumentOutOfRangeException(nameof(balance));

			Balance = balance;
		}

		public event Action Changed;

		public int Balance { get; private set; }

		public void Add(int balance)
		{
			if (balance < 0)
				throw new ArgumentOutOfRangeException(nameof(balance));
			
			Balance += balance;
			Changed?.Invoke();
		}

		public void TrySpend(int balance)
		{
			if (balance < 0)
				throw new ArgumentOutOfRangeException(nameof(balance));

			if (Balance < balance)
				return;

			Balance -= balance;
			Changed?.Invoke();
		}
	}
}