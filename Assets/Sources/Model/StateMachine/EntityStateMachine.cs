using System;
using System.Collections.Generic;
using UnityEngine;

namespace Model.StateMachine
{
	public class EntityStateMachine : ITickable
	{
		private readonly Animator _animator;
		
		private readonly Dictionary<Type, EntityState> _states = new Dictionary<Type, EntityState>();
		private EntityState _currentState = new EntityState.None();

		public EntityStateMachine(Animator animator, IEnumerable<EntityState> states)
		{
			_animator = animator;
			
			foreach (EntityState stickmanState in states)
			{
				Type key = stickmanState.GetType();

				if (_states.ContainsKey(key))
					throw new InvalidOperationException($"Trying to register duplicate state {key}");
				
				_states.Add(key, stickmanState);
			}
		}

		public void Enter<TState>() where TState : EntityState => 
			Enter(typeof(TState));

		public void Enter(Type state)
		{
			if (_states.TryGetValue(state, out var newState) == false)
				throw new InvalidOperationException($"Trying to enter unregistered state {nameof(state)}");

			if (_currentState == newState)
				return;
			
			_currentState.Exit(_animator, this);
			_currentState = newState;
			_currentState.Enter(_animator, this);
		}

		public void Tick(float deltaTime)
		{
			_currentState.Tick(deltaTime, this);
		}
	}
}