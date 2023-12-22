using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class LevelProgress : MonoBehaviour
	{
		[Header("Path")]
		[SerializeField] private Transform _start;
		[SerializeField] private Transform _end;

		[Header("Horde")]
		[SerializeField] private Transform _horde;

		[Header("UI")]
		[SerializeField] private Slider _slider;

		private float _distance;		
		
		private void Start()
		{
			_distance = _end.position.z - _start.position.z;
		}

		private void Update()
		{
			float value = _horde.position.z / _distance;
			_slider.value = value;
		}
	}
}