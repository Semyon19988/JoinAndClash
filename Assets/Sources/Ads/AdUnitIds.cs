using UnityEngine;

namespace Sources.Ads
{
	[UnityEngine.CreateAssetMenu(fileName = "AdUnitIds", menuName = "ScriptableObjects/Ads/AdUnitIds")]
	public class AdUnitIds : ScriptableObject
	{
		[SerializeField] private string _banner;
		[SerializeField] private string _interstitial;
		[SerializeField] private string _rewarded;

		public string Banner => _banner;

		public string Interstitial => _interstitial;

		public string Rewarded => _rewarded;
	}
}