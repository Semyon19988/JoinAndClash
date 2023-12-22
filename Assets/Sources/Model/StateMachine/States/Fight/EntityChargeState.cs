using System;
using System.Collections.Generic;
using Model.StateMachine;
using Model.Stickmen;
using UnityEngine;

namespace Model.Sources.Model.StateMachine.States.FightStates
{
	public class EntityChargeState : EntityFightStatesGroup
	{
		[Serializable]
		public struct Preferences
		{
			public float Speed;
			public float AttackDistance;
		}

		private readonly float _speed;
		private readonly float _attackDistance;

		public EntityChargeState(Entity model, Func<IEnumerable<Entity>> enemiesAlive, Preferences preferences, int animationHash)
			: base(model, enemiesAlive, animationHash)
		{
			_speed = preferences.Speed;
			_attackDistance = preferences.AttackDistance;
		}

		public override void Tick(float deltaTime, EntityStateMachine stateMachine)
		{
			base.Tick(deltaTime, stateMachine);

			Vector3 position = Vector3.MoveTowards(Model.Position, ClosestEnemy.Position, deltaTime * _speed);
			Model.Move(position);
			Model.LookAt(position);
		}

		protected override void CheckTransitions(EntityStateMachine stateMachine)
		{
			base.CheckTransitions(stateMachine);
			
			float sqrMagnitude = (Model.Position - ClosestEnemy.Position).sqrMagnitude;

			if (sqrMagnitude < _attackDistance * _attackDistance)
				OnEnemyApproaching(stateMachine);
		}
		
		protected virtual void OnEnemyApproaching(EntityStateMachine stateMachine)
		{
			stateMachine.Enter<EntityAttackState>();
		}
	}
}