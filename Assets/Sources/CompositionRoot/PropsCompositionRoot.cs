﻿using System;
using System.Collections.Generic;
using Model.Currency;
using Model.Props;
using Model.Stickmen;
using Sources.CompositeRoot.Base;
using Sources.CompositeRoot.Extensions;
using Sources.View.Extensions;
using UnityEngine;
using View.Sources.View.Broadcasters;

namespace Sources.CompositeRoot
{
	public class PropsCompositionRoot : CompositionRoot
	{
		[Header("Audio")] 
		[SerializeField] private AudioClip _pickedSound;

		[Header("Coins")] 
		[SerializeField] private Trigger[] _coins = Array.Empty<Trigger>();
		[SerializeField] private Trigger[] _boosters = Array.Empty<Trigger>();

		[SerializeField] private int _par;

		[Header("Roots")] 
		[SerializeField] private CurrencyCompositionRoot _currencyRoot;
		[SerializeField] private AlliesCompositionRoot _alliesRoot;

		private Wallet Wallet => _currencyRoot.Wallet;
		
		public override void Compose()
		{
			Compose(_coins, () => new Coin(_par), Wallet.Add);
			Compose(_boosters, () => new Booster(), null);
		}

		private void Compose<TModel>(IEnumerable<Trigger> triggers, Func<TModel> construction, Action<TModel> onTrigger)
		{
			foreach (Trigger trigger in triggers)
			{
				TModel model = construction.Invoke();
				trigger.Between<TModel, (StickmanHorde, StickmanMovement)>(model, tuple =>
				{
					onTrigger?.Invoke(model);
					trigger.gameObject.SetActive(false);

					(_, StickmanMovement stickman) = tuple;

					_alliesRoot
						.ViewOf(stickman)
						.GameObject()
						.RequireComponent<AudioSource>()
						.PlayOneShot(_pickedSound);
				});
			}
		}
	}
}