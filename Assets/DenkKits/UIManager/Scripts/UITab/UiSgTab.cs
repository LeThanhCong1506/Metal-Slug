using Imba.UI;
using UnityEngine;
using UnityEngine.UI;

namespace DenkKits.UIManager.Scripts.UITab
{
	[RequireComponent(typeof(Button))]
	[ExecuteInEditMode]
	public class UiSgTab : MonoBehaviour
	{

		public UiSgTabGroup group;
		public UiSgTabBaseEffect effect;
		public bool isDefault;
		public bool autoAddListener = true;
		Button btn;
		UIButton.UIButton btnSg;

		public bool IsCurrentTab
		{
			get
			{
				if (btn != null)
					return !btn.interactable;
				return false;
			}
		}

		void Awake()
		{
			if (btn == null)
			{
				btn = GetComponent<Button>();
				btnSg = GetComponent<UIButton.UIButton>();
				if (autoAddListener)
					btn.onClick.AddListener(OnBaseClick);
			}

			if (isDefault)
				OnBaseClick();
		}

		public bool Interaction
		{
			get { return btn.interactable; }
			set
			{
				if (btn == null)
					return;
				btn.interactable = value;

				if (btnSg != null)
					btnSg.Interactable = value;

				if (effect != null)
					effect.Play(value);
			}
		}

		public void OnBaseClick()
		{
			if (btn == null)
			{
				btn = GetComponent<Button>();
				btn.onClick.AddListener(OnBaseClick);
			}

			if (group != null)
				ActiveMe();
		}

		public void ActiveMe()
		{
			group.ChangeTab(this);
		}
	}
}