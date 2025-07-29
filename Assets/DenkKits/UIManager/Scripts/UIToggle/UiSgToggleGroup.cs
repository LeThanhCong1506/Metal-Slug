using UnityEngine;

namespace DenkKits.UIManager.Scripts.UIToggle
{
	public class UiSgToggleGroup : MonoBehaviour
	{

		UiSgToggle current;

		public UiSgToggle Current
		{
			get { return current; }
		}

		public void ChangeTab(UiSgToggle obj)
		{
			//Debug.LogError (obj.name);
			if (current != null)
				current.Visible = false;
			current = obj;
			current.Visible = true;
		}

		public void HideTab(UiSgToggle obj)
		{
			if (obj == current)
				current = null;
			obj.Visible = false;
		}
	}
}