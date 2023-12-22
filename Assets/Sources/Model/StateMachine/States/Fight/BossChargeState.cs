using System;
using System.Collections.Generic;
using Model.Sources.Model.StateMachine.States.FightStates;
using Model.StateMachine;
using Model.Stickmen;

namespace StateMachine.States.FightStates
{
	public class BossChargeState : EntityChargeState
	{
		public BossChargeState(Entity model, Func<IEnumerable<Entity>> enemiesAlive, Preferences preferences, int animationHash)
			: base(model, enemiesAlive, preferences, animationHash)
		{
			
		}

		protected override void OnEnemyApproaching(EntityStateMachine stateMachine)
		{
			stateMachine.Enter<BossAttackState>();
		}
	}
}