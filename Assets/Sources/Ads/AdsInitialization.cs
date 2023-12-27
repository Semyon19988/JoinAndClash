using UnityEngine;
using UnityEngine.Advertisements;

namespace Sources.Ads
{
	public class AdsInitialization : MonoBehaviour
	{
		[SerializeField] private AdSettings _settings;
		[SerializeField] private AdUnitIds _ids;

		public void Initialize()
		{
			Advertisement.Initialize(_settings.GameId, _settings.TestMode);
			
			Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
			Advertisement.Banner.Show(_ids.Banner);
		}
	}
}