using System;
using System.Collections.Generic;
using Model.Sources.Model.StateMachine.States.FightStates;
using Model.StateMachine;
using Model.Stickmen;
using StateMachine.States.FightStates;
using UnityEngine;

namespace Sources.CompositeRoot.Enemies
{
	public class BossEnemyGroup : EnemyGroup
	{
		[SerializeField] private EntityChargeState.Preferences _bossChargePreferences;
		[SerializeField] private EntityAttackState.Preferences _bossAttackPreferences;
		[SerializeField] private int _bossEnemiesToAttackCount;
		protected override Type ChargeState => typeof(BossChargeState);

		protected override IEnumerable<EntityState> ExtraStates(Entity model, AudioSource audioSource) =>
			new EntityState[]
			{
				new BossChargeState(model, Enemies, _bossChargePreferences, EntityAnimatorParameters.Charge),
				new BossAttackState(model, _bossEnemiesToAttackCount, Enemies, _bossAttackPreferences, audioSource,
					EntityAnimatorParameters.IsPunching),
			};
	}
}