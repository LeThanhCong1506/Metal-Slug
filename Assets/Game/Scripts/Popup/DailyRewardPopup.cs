using System;
using System.Collections.Generic;
using DenkKits.GameServices.Data;
using DenkKits.GameServices.Manager.ResourceManager;
using DenkKits.GameServices.SaveData;
using DenkKits.UIManager.Scripts.UIPopup;
using DG.Tweening;
using Game.Scripts.Popup.Daily_Reward;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Popup
{
    public class DailyRewardPopup : UIPopup
    {
        public event Action OnClaimedEvent;
        [SerializeField] private Button claimButton, claimx2Button, closeButton;

        [SerializeField]
        private GameObject normalClaimAdImage, normalClaimTicketImage, rewardAdImage, rewardTicketImage;

        [SerializeField] private ParticleSystem fireworkVFX, firework2VFX;

        [SerializeField] private List<DailyRewardCell> itemList;

        private int _price;
        private RewardBonusList _rewardBonusList;
        private DailyRewardCell _currentCell;

        protected override void OnShowing()
        {
            base.OnShowing();
            UpdateUI();
        }

        private void UpdateUI()
        {
            UpdateDay();
            UpdateItems();
        }

        private void UpdateItems()
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                var item = itemList[i];
                var data = ResourceManager.Instance.dailyRewardCollection[i];
                item.Init(i, data, OnItemSelected);
            }

            _currentCell = itemList[SaveDataHandler.Instance.saveData.currentDayDailyReward - 1];
            claimButton.interactable = !_currentCell.isClaimed;
            claimx2Button.interactable = !_currentCell.isClaimed;
        }

        private void UpdateDay()
        {
            int totalDays = (int)(DateTime.Now - SaveDataHandler.Instance.GetLastTimeEarnedDailyReward()).TotalDays;
            if (totalDays >= 1)
            {
                SaveDataHandler.Instance.saveData.currentDayDailyReward++;
            }

            if (SaveDataHandler.Instance.saveData.currentDayDailyReward > 7)
            {
                ResetDailyReward();
            }

        }

        private void ResetDailyReward()
        {
            SaveDataHandler.Instance.saveData.currentDayDailyReward = 1;
            SaveDataHandler.Instance.ResetDailyRewardList();
            SaveDataHandler.Instance.ResetLastTimeEarnedDailyReward();
        }

        private void OnItemSelected(DailyRewardCell cell)
        {
            if (_currentCell != null && _currentCell != cell)
            {
                _currentCell.isSelected = false;
                _currentCell.UpdateUI();
            }

            _currentCell = cell;
            SaveDataHandler.Instance.saveData.currentDayDailyReward = _currentCell.dayIndex;
            claimButton.interactable = !_currentCell.isClaimed;
            claimx2Button.interactable = !_currentCell.isClaimed;
            UpdateUI();
        }

        [Button]
        private void PlayVFX()
        {
            fireworkVFX.Play();
            firework2VFX.Play();
        }

        private void NormalClaim()
        {
            var item = itemList[SaveDataHandler.Instance.saveData.currentDayDailyReward - 1];
            if (item.isWatchAd)
            {
                // NORMAL REWARD BUT ADS
                claimButton.interactable = false;
                claimx2Button.interactable = false;
                item.ClaimReward(false);
                PlayVFX();
                OnClaimedEvent?.Invoke();
                DOVirtual.DelayedCall(2.5f, () => Hide());
            }
            else
            {
                // NORMAL CLAIM 
                claimButton.interactable = false;
                claimx2Button.interactable = false;
                item.ClaimReward(false);
                PlayVFX();
                OnClaimedEvent?.Invoke();
                DOVirtual.DelayedCall(2.5f, () => Hide());
            }
        }

        private void AdRewardClaim()
        {
            claimButton.interactable = false;
            claimx2Button.interactable = false;
            itemList[SaveDataHandler.Instance.saveData.currentDayDailyReward - 1].ClaimReward(true);
            PlayVFX();
            OnClaimedEvent?.Invoke();
            DOVirtual.DelayedCall(2.5f, () => Hide());
        }

    }
}