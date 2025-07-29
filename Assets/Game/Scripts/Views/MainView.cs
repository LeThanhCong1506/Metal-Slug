using AssetKits.ParticleImage;
using DenkKits.GameServices.Audio.Scripts;
using DenkKits.GameServices.Manager;
using DenkKits.UIManager.Scripts.Base;
using DenkKits.UIManager.Scripts.UIPopup;
using DenkKits.UIManager.Scripts.UIView;
using Game.Scripts.Popup;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts.Views
{
    public class MainView : UIView
    {
        [Header("Main View")] 
        [SerializeField] private GameObject mainMenuGroup;
        [SerializeField] private ParticleImage cParticleImage;

        public UnityAction onClickShowSomeThing;
        public UnityAction onClickBuySomethingSuccess;
        public UnityAction onLoadGame;
        public UnityAction onClickBackToMain;

        protected override void OnShowing()
        {
            base.OnShowing();
            OnClickBackToMain();
            SoArchitectureManager.Instance.OnMoneyChangedEvent.AddListener(OnCoinChanged);
        }

        protected override void OnHiding()
        {
            base.OnHiding();
            SoArchitectureManager.Instance.OnMoneyChangedEvent.RemoveListener(OnCoinChanged);
        }

        #region MAIN UI BUTTON CALLBACK

        public void OnCoinChanged()
        {
            
        }

        public void OnClickUpgrade()
        {
            UIManager.Instance.PopupManager.ShowPopup(UIPopupName.UpgradePopup);
        }

        public void OnClickBackToMain()
        {
            mainMenuGroup.SetActive(true);
            onClickBackToMain?.Invoke();
        }

        public void OnClickShowSomething()
        {
            mainMenuGroup.SetActive(false);
            onClickShowSomeThing?.Invoke();
        }


        public void OnClickBuySomethingConfirm()
        {
            onClickBuySomethingSuccess?.Invoke();
            cParticleImage.Play();
            OnClickBackToMain();
        }

        public void OnClickPlayGame()
        {
            onLoadGame?.Invoke();
        }

        public void OnClickOpenSetting()
        {
            var param = new SettingPopupParam
            {
                showGroupBtn = false
            };
            AudioManager.Instance.PlaySfx(AudioName.UI_Click);
            UIManager.Instance.PopupManager.ShowPopup(UIPopupName.SettingPopup, param);
        }


        public void OnClickLeaderBoard()
        {
            AudioManager.Instance.PlaySfx(AudioName.UI_Click);
            UIManager.Instance.PopupManager.ShowPopup(UIPopupName.LeaderBoardPopup);
        }

        public void OnClickHowToPlay()
        {
            AudioManager.Instance.PlaySfx(AudioName.UI_Click);
            UIManager.Instance.PopupManager.ShowPopup(UIPopupName.TutorialPopup);
        }

        #endregion
    }
}