using System;
using System.Collections.Generic;
using Model.Sources.Model.StateMachine.States.FightStates;
using Model.StateMachine;
using Model.Stickmen;
using UnityEngine;

namespace Sources.CompositeRoot.Enemies
{
	public class StickmanEnemyGroup : EnemyGroup
	{
		[SerializeField] private EntityChargeState.Preferences _stickmanChargePreferences;
		[SerializeField] private EntityAttackState.Preferences _stickmanAttackPreferences;

		protected override Type ChargeState => typeof(EntityChargeState);

		protected override IEnumerable<EntityState> ExtraStates(Entity model, AudioSource audioSource) =>
			new EntityState[]
			{
				new EntityChargeState(model, Enemies, _stickmanChargePreferences, EntityAnimatorParameters.Charge),
				new EntityAttackState(model, Enemies, _stickmanAttackPreferences, audioSource,
					EntityAnimatorParameters.IsPunching),
			};
	}
}
	