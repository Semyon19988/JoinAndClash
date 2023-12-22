using System.Threading.Tasks;
using GameStates.Base;
using GameStates.States;
using StaticContext;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using Sources.Ads;

namespace UI
{
	[RequireComponent(typeof(Button))]
	public class ExitButton : MonoBehaviour, IUnityAdsShowListener
	{
		[SerializeField] private AdUnitIds _ids;
		private void Start()
		{
			Button button = GetComponent<Button>();
			button.onClick.AddListener(() => Advertisement.Show(_ids.Interstitial, this));
		}

		public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
		{
			Instance<IGameStateMachine>.Value.Enter<BootstrapState>();
		}

		public async void OnUnityAdsShowStart(string placementId)
		{
			await Task.Delay(1000);
			
			Instance<IGameStateMachine>.Value.Enter<BootstrapState>();
		}

		public void OnUnityAdsShowClick(string placementId)
		{
		}

		public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
		{
			Instance<IGameStateMachine>.Value.Enter<BootstrapState>();
		}
	}
}