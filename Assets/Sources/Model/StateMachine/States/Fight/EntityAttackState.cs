using System;
using System.Collections.Generic;
using Model.StateMachine;
using Model.Stickmen;
using Model.Timers;
using UnityEngine;

namespace Model.Sources.Model.StateMachine.States.FightStates
{
	public class EntityAttackState : EntityFightStatesGroup
	{
		[Serializable]
		public struct Preferences
		{
			public float Damage;
			public float TimeBetweenAttacks;
			public AudioClip PunchSound;
		}

		private readonly float _damage;
		private readonly float _timeBetweenAttacks;
		private readonly AudioClip _punchSound;
		private readonly AudioSource _audioSource;
		
		private readonly Timer _timer = new Timer();
		
		public EntityAttackState(Entity model,
									Func<IEnumerable<Entity>> enemiesAlive,
									Preferences preferences,
									AudioSource audioSource,
									int animationHash) 
			: base(model, enemiesAlive, animationHash)
		{
			_damage = preferences.Damage;
			_timeBetweenAttacks = preferences.TimeBetweenAttacks;
			_punchSound = preferences.PunchSound;
			_audioSource = audioSource;
		}

		public override void Tick(float deltaTime, EntityStateMachine stateMachine)
		{
			base.Tick(deltaTime, stateMachine);

			if (_timer.IsOver) 
				Punch(EnemiesToPunch());

			_timer.Tick(deltaTime);
		}

		protected override void CheckTransitions(EntityStateMachine stateMachine)
		{
			base.CheckTransitions(stateMachine);
			
			if (ClosestEnemy.IsDead)
				OnEnemyDied(stateMachine);
		}
		
		protected virtual void OnEnemyDied(EntityStateMachine stateMachine)
		{
			stateMachine.Enter<EntityChargeState>();
		}

		protected virtual IEnumerable<Entity> EnemiesToPunch()
		{
			return new[] {ClosestEnemy};
		}


		private void Punch(IEnumerable<Entity> enemies)
		{
			foreach (Entity enemy in enemies)
			{
				enemy.TakeDamage(_damage);
			}

			_audioSource.PlayOneShot(_punchSound);
			_timer.Start(_timeBetweenAttacks);
		}
	}
}