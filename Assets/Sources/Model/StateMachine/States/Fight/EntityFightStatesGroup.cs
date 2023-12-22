using System;
using System.Collections.Generic;
using System.Linq;
using Model.Extensions;
using Model.StateMachine;
using Model.StateMachine.States;
using Model.Stickmen;
using UnityEngine;

namespace Model.Sources.Model.StateMachine.States.FightStates
{
	public abstract class EntityFightStatesGroup : EntityState
	{
		protected readonly Entity Model;
		private readonly Func<IEnumerable<Entity>> _enemiesAlive;

		protected EntityFightStatesGroup(Entity model, Func<IEnumerable<Entity>> enemiesAlive, int animationHash)
			: base(animationHash)
		{
			Model = model;
			_enemiesAlive = enemiesAlive;
		}
		
		protected Entity ClosestEnemy { get; private set; }

		protected IEnumerable<Entity> EnemiesAlive => _enemiesAlive.Invoke();

		public override void Enter(Animator animator, EntityStateMachine stateMachine)
		{
			base.Enter(animator, stateMachine);

			IEnumerable<Entity> enemies = _enemiesAlive.Invoke();

			if (enemies.Any() == false)
			{
				stateMachine.Enter<EntityVictoryState>();
				return;
			}

			ClosestEnemy = enemies.ClosestTo(Model);
		}
	}
}