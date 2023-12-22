using GameStates.Base;
using UnityEngine;

namespace GameStates.States
{
	public class PauseState : IGameState
	{
		private const float PausedTimeScale = 0.0f;
		private float _timeScale;
		
		public void Enter()
		{
			_timeScale = Time.timeScale;
			Time.timeScale = PausedTimeScale;
		}

		public void Exit()
		{
			Time.timeScale = _timeScale;
		}
	}
}