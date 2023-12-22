using System;
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
		[SerializeField] [Min(1)] private int _par;
		[SerializeField] private Trigger[] _coins = Array.Empty<Trigger>();

		[Header("Boosters")] 
		[SerializeField] private Booster.Preferences _preferences;
		[SerializeField] private Trigger[] _boosters = Array.Empty<Trigger>();

		[Header("Roots")]
		[SerializeField] private CurrencyCompositionRoot _currencyRoot;
		[SerializeField] private AlliesCompositionRoot _alliesRoot;
		[SerializeField] private HordeCompositionRoot _hordeRoot;
		
		private Wallet Wallet => _currencyRoot.Wallet;

		private StickmanHordeMovement HordeMovement => _hordeRoot.HordeMovement;

		private readonly List<Booster> _boostersToTick = new List<Booster>();

		public override void Compose()
		{
			Compose(_coins, () => new Coin(_par), Wallet.Add);
			Compose(_boosters, () => new Booster(_preferences, HordeMovement), booster =>
			{
				booster.Apply();
				HordeMovement.Bind(booster);
				_boostersToTick.Add(booster);
			});
		}

		private void Update() => 
			_boostersToTick.ForEach(x => x.Tick(Time.deltaTime));

		private void Compose<TModel>(IEnumerable<Trigger> triggers, Func<TModel> construction, Action<TModel> onTriggerEnter)
		{
			foreach (Trigger trigger in triggers)
			{
				TModel model = construction.Invoke();

				trigger.Between<TModel, (StickmanHorde, StickmanMovement)>(model, tuple =>
				{
					onTriggerEnter?.Invoke(model);
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