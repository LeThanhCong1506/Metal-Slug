using System;
using System.Collections.Generic;
using DenkKits.GameServices.Data;
using DenkKits.GameServices.SaveData;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Popup.Daily_Reward
{
    public class DailyRewardCell : MonoBehaviour
    {
        public bool isClaimed;
        public bool isSelected;
        public bool isAvailable;
        public bool isWatchAd;

        public int dayIndex;

        public event Action<DailyRewardCell> OnSelectedEvent;
        [SerializeField] private DailyRewardItem templatePrefab;
        [SerializeField] private RectTransform contain;
        [SerializeField] private TextMeshProUGUI textDay;
        [SerializeField] private Image greenTick, notAvailableImage, claimedImage;
        [SerializeField] private Button button;
        [SerializeField] private DOTweenAnimation borderSelected;

        private readonly List<DailyRewardItem> _rewardBonusDataList = new();

        private RewardBonusList _rewardBonusList;

        private void Start()
        {
            button.onClick.AddListener(OnSelected);
        }

        public void Init(int day, RewardBonusList rewardBonusList, Action<DailyRewardCell> onSelected)
        {
            dayIndex = day + 1;
            _rewardBonusList = rewardBonusList;
            OnSelectedEvent = onSelected;

            isAvailable = dayIndex <= SaveDataHandler.Instance.saveData.currentDayDailyReward;
            isClaimed = SaveDataHandler.Instance.IsDayDailyRewardClaimed(dayIndex);

            if (dayIndex < SaveDataHandler.Instance.saveData.currentDayDailyReward)
            {
                // isWatchAd = true;
            }
            else if (dayIndex == SaveDataHandler.Instance.saveData.currentDayDailyReward)
            {
                isSelected = true;
            }

            for (int i = 0; i < _rewardBonusList.rewardList.Count; i++)
            {
                var item = Instantiate(templatePrefab, contain);
                item.Init(_rewardBonusList.rewardList[i], isClaimed);
                _rewardBonusDataList.Add(item);
            }

            UpdateUI();
        }

        public void UpdateUI()
        {
            textDay.text = "Day " + dayIndex;
            notAvailableImage.gameObject.SetActive(!isAvailable);
            claimedImage.gameObject.SetActive(isClaimed);

            if (isAvailable)
            {
                greenTick.gameObject.SetActive(isClaimed);
                button.interactable = !isClaimed;
                if (!isClaimed && isSelected)
                {
                    borderSelected.gameObject.SetActive(true);
                }
                else
                {
                    borderSelected.gameObject.SetActive(false);
                }
            }
            else
            {
                borderSelected.gameObject.SetActive(false);
                greenTick.gameObject.SetActive(false);
                button.interactable = false;
            }
        }

        private void OnSelected()
        {
            OnSelectedEvent?.Invoke(this);
            isSelected = true;
            UpdateUI();
        }

        public void ClaimReward(bool isMultiplyReward)
        {
            if (isClaimed)
            {
                return;
            }

            SaveDataHandler.Instance.ResetLastTimeEarnedDailyReward();
            SaveDataHandler.Instance.AddDayDailyRewardClaimed(SaveDataHandler.Instance.saveData
                .currentDayDailyReward);

            isClaimed = true;
            UpdateUI();

            if (_rewardBonusDataList.Count == 0)
            {
                return;
            }

            for (int i = 0; i < _rewardBonusDataList.Count; i++)
            {
                int amount = isMultiplyReward ? _rewardBonusDataList[i].amount * 2 : _rewardBonusDataList[i].amount;
                // GameManager.Instance.AddCurrencyRewardWithParticle(amount,
                //     _rewardBonusDataList[i].GetRewardBonusData().type);
            }
        }
    }
}