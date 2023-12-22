using System;
using System.Collections.Generic;
using System.Linq;
using Model.Components;
using Model.StateMachine;
using Model.StateMachine.States;
using Model.Stickmen;
using Sources.CompositeRoot.Extensions;
using Sources.View;
using Sources.View.Extensions;
using UnityEngine;
using View.Sources.View.Broadcasters;

namespace Sources.CompositeRoot.Enemies
{
	public abstract class EnemyGroup : MonoBehaviour
	{
		[Header("Common preferences")]
		[SerializeField] private float _health;

		[Header("Roots")]
		[SerializeField] private HordeCompositionRoot _hordeRoot;
		
		[Header("Scene Events")]
		[SerializeField] private EventTrigger _pathFinishTrigger;

		[Header("Used Assets")]
		[SerializeField] private RuntimeAnimatorController _controller;

		[Header("Audio")] 
		[SerializeField] private AudioClip _deathSound;
		
		[Header("Views")]
		[SerializeField] private PhysicsTransformableView[] _enemies = Array.Empty<PhysicsTransformableView>();
		
		public IEnumerable<Entity> Models() =>
			_enemies.Select(BindModel);
		
		protected abstract IEnumerable<EntityState> ExtraStates(Entity model, AudioSource audioSource);

		protected abstract Type ChargeState { get; }

		protected Func<IEnumerable<Entity>> Enemies => _hordeRoot.Entities;

		private Entity BindModel(PhysicsTransformableView view)
		{
			var health = new Health(_health);
			var model = new Entity(health, view.transform.position, view.transform.rotation);

			view
				.Initialize(model)
				.RequireComponent<Animator>(out var animator)
				.BindController(_controller)
				.RequireComponent<AudioSource>(out var audioSource)
				.GameObject()
				.AddComponent<TickBroadcaster>()
				.InitializeAs(new EntityStateMachine(animator, new EntityState[]
					{
						new EntityWaitState(EntityAnimatorParameters.Idle),
						new EntityDeathState(audioSource, _deathSound, EntityAnimatorParameters.IsDead),
						new EntityVictoryState(EntityAnimatorParameters.Won)
					}.Union(ExtraStates(model, audioSource))),
					out var stateMachine)
				.ContinueWith(stateMachine.Enter<EntityWaitState>)
				.OnTrigger(_pathFinishTrigger)
				.ReactOnce(() => stateMachine.Enter(ChargeState))
				.ContinueWith(() => model.Died += stateMachine.Enter<EntityDeathState>);

			return model;
		}
	}
}