using UnityEngine;

namespace DenkKits.UIManager.Scripts.UIToggle
{
	public sealed class UiSgToggleEffect : MonoBehaviour
	{

		public void Visable()
		{
			gameObject.SetActive(true);
		}

		public void Disable()
		{
			gameObject.SetActive(false);
		}
	}
}