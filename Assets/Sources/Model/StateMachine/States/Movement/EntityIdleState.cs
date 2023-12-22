using Model.StateMachine;
using Model.Stickmen;

namespace Model.Sources.Model.StateMachine.States.Movement
{
	public class EntityIdleState : EntityMoveStatesGroup
	{
		public EntityIdleState(StickmanMovement movement, int animationHash)
			: base(movement, animationHash) { }

		protected override void CheckTransitions(EntityStateMachine stateMachine)
		{
			base.CheckTransitions(stateMachine);
			
			if (IsRunning)
				stateMachine.Enter<EntityRunState>();
		}
	}
}