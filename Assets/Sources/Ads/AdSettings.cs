using UnityEngine;

namespace Sources.Ads
{
	[CreateAssetMenu(fileName = "AdSettings", menuName = "ScriptableObjects/Ads/AdSettings")]
	public class AdSettings : ScriptableObject
	{
		[SerializeField] private string _gameId;
		[SerializeField] private bool _testMode;

		public string GameId => _gameId;

		public bool TestMode => _testMode;
	}
}