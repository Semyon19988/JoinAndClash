using UnityEngine;

namespace Model.StateMachine.States
{
	public class EntityDeathState : EntityState
	{
		private readonly AudioSource _audioSource;
		private readonly AudioClip _deathSound;

		public EntityDeathState(AudioSource audioSource, AudioClip deathSound, int animationHash) : base(animationHash)
		{
			_audioSource = audioSource;
			_deathSound = deathSound;
		}

		public override void Enter(Animator animator, EntityStateMachine stateMachine)
		{
			base.Enter(animator, stateMachine);
			
			_audioSource.PlayOneShot(_deathSound);
		}
	}
}