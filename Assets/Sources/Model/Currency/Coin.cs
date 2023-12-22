using System;

namespace Model.Currency
{
	public class Coin
	{
		public readonly int Par;

		public Coin(int par)
		{
			if (par <= 0)
				throw new ArgumentOutOfRangeException(nameof(par));

			Par = par;
		}
	}
}