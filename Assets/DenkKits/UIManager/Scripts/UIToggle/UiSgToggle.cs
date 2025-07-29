using Imba.UI;
using UnityEngine;

namespace DenkKits.UIManager.Scripts.UIToggle
{
	public class UiSgToggle : MonoBehaviour
	{

		public UiSgToggleGroup group;
		public UiSgToggleEffect effect;

		void Awake()
		{
			if (group == null)
				Debug.LogError(name);
			if (gameObject.activeSelf)
			{
				if (group.Current == null)
					Show();
				else if (group.Current != this)
					gameObject.SetActive(false);
			}
		}

		public bool Visible
		{
			set
			{

				if (effect != null)
				{
					if (value)
						effect.Visable();
					else
						effect.Disable();
				}
				else
					gameObject.SetActive(value);
			}
			get { return gameObject.activeSelf; }
		}

		public void Show()
		{
#if UNITY_EDITOR
		if(group == null)
			Debug.LogError (gameObject.name);
		#endif
			group.ChangeTab(this);
		}

		public void Hide()
		{
			group.HideTab(this);
		}
	}
}