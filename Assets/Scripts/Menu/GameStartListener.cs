using GameStates.Base;
using GameStates.States;
using Touch = Input.Touches.Touch;
using Input.Touches;
using UnityEngine;
using StaticContext;

namespace Menu
{
	public class GameStartListener : MonoBehaviour
	{
		[SerializeField] private InputTouchPanel _startGamePanel;

		private void OnEnable() =>
			_startGamePanel.Begun += EnterGameplayState;

		private void OnDisable() =>
			_startGamePanel.Begun -= EnterGameplayState;

		private void EnterGameplayState(Touch touch) =>
			Instance<IGameStateMachine>
				.Value
				.Enter<GameplayState>();
	}
}