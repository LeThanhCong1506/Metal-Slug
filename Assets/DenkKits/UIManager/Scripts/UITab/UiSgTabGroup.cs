using UnityEngine;

namespace DenkKits.UIManager.Scripts.UITab
{
	public class UiSgTabGroup : MonoBehaviour
	{

		UiSgTab current;

		public UiSgTab Current
		{
			get { return current; }
		}

		public void ChangeTab(UiSgTab tab)
		{
			if (current != null)
				current.Interaction = true;
			current = tab;
			if (tab != null)
				tab.Interaction = false;
		}
	}
}