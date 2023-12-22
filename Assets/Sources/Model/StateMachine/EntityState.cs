using UnityEngine;

namespace Model.StateMachine
{
	public abstract class EntityState
	{
		private readonly int _animationHash;

		protected EntityState(int animationHash)
		{
			_animationHash = animationHash;
		}

		public virtual void Enter(Animator animator, EntityStateMachine stateMachine)
		{
			animator.SetBool(_animationHash, true);
		}

		public virtual void Exit(Animator animator, EntityStateMachine stateMachine)
		{
			animator.SetBool(_animationHash, false);
		}

		public virtual void Tick(float deltaTime, EntityStateMachine stateMachine)
		{
			CheckTransitions(stateMachine);
		}

		protected virtual void CheckTransitions(EntityStateMachine stateMachine)
		{

		}

		public class None : EntityState
		{
			public None() : base(0)
			{
			}

			public override void Enter(Animator animator, EntityStateMachine stateMachine)
			{
			}

			public override void Exit(Animator animator, EntityStateMachine stateMachine)
			{
			}
		}
	}
}