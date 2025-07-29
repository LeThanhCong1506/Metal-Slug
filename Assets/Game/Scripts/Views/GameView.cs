using DenkKits.GameServices.Audio.Scripts;
using DenkKits.UIManager.Scripts.Base;
using DenkKits.UIManager.Scripts.UIPopup;
using DenkKits.UIManager.Scripts.UIView;
using Game.Scripts.Popup;
using UnityEngine;

namespace Game.Scripts.Views
{
    public class GameView : UIView
    {
        [SerializeField] private FloatingJoystick joystick;
        public Joystick Joystick => joystick;

        public void OpenSetting()
        {
            var param = new SettingPopupParam
            {
                showGroupBtn = true
            };
            AudioManager.Instance.PlaySfx(AudioName.UI_Click);
            UIManager.Instance.PopupManager.ShowPopup(UIPopupName.SettingPopup, param);
        }
    }
}