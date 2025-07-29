using AssetKits.ParticleImage;
using DenkKits.GameServices.Audio.Scripts;
using DenkKits.GameServices.SaveData;
using DenkKits.UIManager.Scripts.Base;
using DenkKits.UIManager.Scripts.UIPopup;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts.Popup
{
    public class EndGamePopupParam
    {
    }

    public class EndGamePopup : UIPopup
    {
        [SerializeField] private ParticleImage moneyParticleImage;

        private int userCoinEarn;
        protected override void OnInit()
        {
            base.OnInit();
            moneyParticleImage.onFirstParticleFinished.AddListener(OnMoneyFirstParticleFinished);
        }


        protected override void OnShowing()
        {
            base.OnShowing();
        }


        public void OnCLickNext()
        {
            AudioManager.Instance.PlaySfx(AudioName.Gameplay_CoinEndGame);
            moneyParticleImage.SetActive(true);
            moneyParticleImage.Play();
        }


        private void OnMoneyFirstParticleFinished()
        {
            int oldMoney = SaveDataHandler.Instance.saveData.moneyAmount;
            int newMoney = oldMoney + userCoinEarn;

            SaveDataHandler.Instance.saveData.moneyAmount = newMoney;

            DOTween.To(() => oldMoney, x =>
            {
                oldMoney = x;
            }, newMoney, 1f).OnComplete(() =>
            {
                // High score check
                var curHigh = SaveDataHandler.Instance.saveData.userHighScore;
                if (curHigh < userCoinEarn)
                {
                    SaveDataHandler.Instance.saveData.userHighScore = userCoinEarn;
                }
                SaveDataHandler.Instance.RequestSave();
                Invoke(nameof(OnClickExit), 0.5f);
            });
        }

        public void OnClickExit()
        {
            Hide();
            UIManager.Instance.ShowTransition(() => { SceneManager.LoadScene(GameConstants.SceneMain); });
        }
    }
}