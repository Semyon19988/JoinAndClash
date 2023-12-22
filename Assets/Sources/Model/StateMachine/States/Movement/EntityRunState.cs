using Model.StateMachine;
using Model.Stickmen;

namespace Model.Sources.Model.StateMachine.States.Movement
{
	public class EntityRunState : EntityMoveStatesGroup
	{
		public EntityRunState(StickmanMovement movement, int animationHash) 
			: base(movement, animationHash) { }

		protected override void CheckTransitions(EntityStateMachine stateMachine)
		{
			base.CheckTransitions(stateMachine);
			
			if (IsRunning == false)
				stateMachine.Enter<EntityIdleState>();
		}
	}
}