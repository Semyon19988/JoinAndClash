using System;
using System.Collections.Generic;
using System.Linq;
using Model.Sources.Model.StateMachine.States.FightStates;
using Model.StateMachine;
using Model.Stickmen;
using UnityEngine;

namespace StateMachine.States.FightStates
{
	public class BossAttackState : EntityAttackState
	{
		private readonly int _enemiesCount;
		
		public BossAttackState(Entity model,
								int enemiesCount,
								Func<IEnumerable<Entity>> enemiesAlive,
								Preferences preferences,
								AudioSource audioSource,
								int animationHash)
			: base(model, enemiesAlive, preferences, audioSource, animationHash)
		{
			_enemiesCount = enemiesCount;
		}

		protected override IEnumerable<Entity> EnemiesToPunch() => 
			EnemiesAlive
				.OrderBy(x => (x.Position - Model.Position).sqrMagnitude)
				.Take(_enemiesCount);

		protected override void OnEnemyDied(EntityStateMachine stateMachine)
		{
			stateMachine.Enter<BossChargeState>();
		}
	}
}