using DenkKits.GameServices.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Popup.Daily_Reward
{
    public class DailyRewardItem : MonoBehaviour
    {
        [SerializeField] private Image bonusImage;
        [SerializeField] private TextMeshProUGUI amountText;

        public int amount;

        private RewardBonusData _rewardBonusData;

        public void Init(RewardBonusData data, bool isClaimed)
        {
            _rewardBonusData = data;
            amount = _rewardBonusData.amount;

            switch (_rewardBonusData.type)
            {
                case AuxiliaryType.Booster1:
                    // SET sprite
                    // bonusImage.sprite = ResourceManager.Instance.AuxiliaryDataCollection.GetData(AuxiliaryType.Helicopter).Sprite;
                    amountText.text = "x" + amount;
                    break;
              
            }
        }

        public RewardBonusData GetRewardBonusData() => _rewardBonusData;
    }
}