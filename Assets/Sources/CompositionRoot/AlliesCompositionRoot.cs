using System;
using System.Collections.Generic;
using Model;
using Model.Components;
using Model.Obstacles;
using Model.Physics;
using Model.Sources.Model.Movement;
using Model.Sources.Model.StateMachine.States.FightStates;
using Model.Sources.Model.StateMachine.States.Movement;
using Model.StateMachine;
using Model.StateMachine.States;
using Model.Stickmen;
using Sources.CompositeRoot.Base;
using Sources.CompositeRoot.Extensions;
using Sources.CompositeRoot.Enemies;
using Sources.View;
using Sources.View.Extensions;
using UnityEditor.Animations;
using UnityEngine;
using View.Sources.View.Broadcasters;

namespace Sources.CompositeRoot
{
	public class AlliesCompositionRoot : CompositionRoot
	{
		[Header("Roots")] 
		[SerializeField] private EnemiesCompositionRoot _enemiesRoot;
		
		[Header("Preferences")]
		[SerializeField] private float _distanceBetweenBounds;
		[SerializeField] private float _health;
		[SerializeField] private EntityChargeState.Preferences _chargePreferences;
		[SerializeField] private EntityAttackState.Preferences _attackPreferences;
		
		[Header("Used assets")]
		[SerializeField] private AnimatorController _controller;
		[SerializeField] private CapsuleCollider _pickTriggerZonePrefab;

		[Header("Scene")] 
		[SerializeField] private EventTrigger _pathFinishTrigger;
		[SerializeField] private string _groundTag;

		[Header("Audio")] 
		[SerializeField] private AudioClip _pickSound;
		[SerializeField] private AudioClip _deathSound;

		[Header("Views")]
		[SerializeField] private PhysicsTransformableView _playerView;
		[SerializeField] private PhysicsTransformableView[] _otherViews = Array.Empty<PhysicsTransformableView>();

		private Dictionary<StickmanMovement, PhysicsTransformableView> _placedEntities;

		public override void Compose()
		{
			_placedEntities = new Dictionary<StickmanMovement, PhysicsTransformableView>(_otherViews.Length);
			
			Player = Compose(_playerView);
			_placedEntities.Add(Player, _playerView);

			foreach (PhysicsTransformableView view in _otherViews)
			{
				StickmanMovement movement = Compose(view);
				_placedEntities.Add(movement, view);
			}
		}

		public IReadOnlyDictionary<StickmanMovement, PhysicsTransformableView> PlacedEntities => _placedEntities;

		public StickmanMovement Player { get; private set; }

		public PhysicsTransformableView ViewOf(StickmanMovement stickman) => 
			_placedEntities[stickman];

		private StickmanMovement Compose(PhysicsTransformableView view)
		{
			var health = new Health(_health);
			var model = new Entity(health, view.transform.position, view.transform.rotation);
			var inertialMovement = new InertialMovement(new IMovementStatsProvider.None());
			var surfaceSliding = new SurfaceSliding(_groundTag);
			var movement = new StickmanMovement(model, surfaceSliding, inertialMovement, _distanceBetweenBounds);

			view
				.Initialize(model)
				.AddComponent<CollisionBroadcaster>()
				.Initialize(surfaceSliding)
				.RequireComponent<Animator>(out var animator)
				.BindController(_controller)
				.RequireComponent<AudioSource>(out var audioSource)
				.GameObject()
				.AddComponent<TickBroadcaster>()
				.InitializeAs(new EntityStateMachine(animator, new EntityState[]
				{
					new EntityWaitState(EntityAnimatorParameters.Idle),
					new EntityIdleState(movement, EntityAnimatorParameters.Idle),
					new EntityRunState(movement, EntityAnimatorParameters.IsRunning),
					new EntityChargeState(model, _enemiesRoot.Entities, _chargePreferences, EntityAnimatorParameters.Charge),
					new EntityAttackState(model, _enemiesRoot.Entities, _attackPreferences, audioSource, EntityAnimatorParameters.IsPunching),
					new EntityDeathState(audioSource, _deathSound, EntityAnimatorParameters.IsDead),
					new EntityVictoryState(EntityAnimatorParameters.Won)
				}), out var stateMachine)
				.ContinueWith(stateMachine.Enter<EntityIdleState>)
				.Append(_pickTriggerZonePrefab)
				.GoToParent()
				.AddComponent<Trigger>()
				.Between<StickmanMovement, (StickmanHorde, StickmanMovement)>(movement, handler =>
				{
					handler.Item1.Add(movement);
					audioSource.PlayOneShot(_pickSound);
				})
				.OnTrigger(_pathFinishTrigger)
				.Do(stateMachine.Enter<EntityChargeState>)
				.ContinueWith(() => model.Died += stateMachine.Enter<EntityDeathState>);

			return movement;
		}
	}
}