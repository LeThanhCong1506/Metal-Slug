using UnityEngine;

namespace DenkKits.UIManager.Scripts.UITab
{
	public class UiSgTabBaseEffect : MonoBehaviour
	{


		public void Play(bool enable)
		{
			if (enable)
				Enable();
			else
				Disable();
		}

		protected virtual void Enable()
		{

		}

		protected virtual void Disable()
		{

		}
	}
}