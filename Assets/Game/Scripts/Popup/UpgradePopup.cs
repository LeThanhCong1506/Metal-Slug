using System;
using System.Collections.Generic;
using DenkKits.GameServices.Manager;
using DenkKits.GameServices.SaveData;
using DenkKits.UIManager.Scripts.UIPopup;
using Game.Scripts.Popup.UpgradeComps;
using TMPro;
using UnityEngine;

namespace Game.Scripts.Popup
{
    public class UpgradePopup : UIPopup
    {
        [Serializable]
        public class UpgradeItemRef
        {
            public PlayerUpgradeType upgradeType;
            public UpgradeItem upgradeItem;
        }

        [SerializeField] private TextMeshProUGUI textCoin;
        [SerializeField] private List<UpgradeItemRef> upgradeItems;

        protected override void OnShowing()
        {
            base.OnShowing();
            RefreshUI();
        }

        private int GetUpgradeLevel(PlayerUpgradeType type)
        {
            var save = SaveDataHandler.Instance.saveData;
            return type switch
            {
                PlayerUpgradeType.Capacity => save.capacityLevel,
                PlayerUpgradeType.Fuel => save.fuelLevel,
                PlayerUpgradeType.Speed => save.speedLevel,
                PlayerUpgradeType.Range => save.suctionRangeLevel,
                _ => 0
            };
        }

        private void SetUpgradeLevel(PlayerUpgradeType type, int newLevel)
        {
            var save = SaveDataHandler.Instance.saveData;
            switch (type)
            {
                case PlayerUpgradeType.Capacity: save.capacityLevel = newLevel; break;
                case PlayerUpgradeType.Fuel: save.fuelLevel = newLevel; break;
                case PlayerUpgradeType.Speed: save.speedLevel = newLevel; break;
                case PlayerUpgradeType.Range: save.suctionRangeLevel = newLevel; break;
            }
        }

        private int GetUpgradeCost(PlayerUpgradeType type, int currentLevel)
        {
            return type switch
            {
                _ => 0
            };
        }

        private string GetCurrentValueString(PlayerUpgradeType type, int currentLevel)
        {
            return type switch
            {
                _ => "0"
            };
        }

        public void RefreshUI()
        {
            var save = SaveDataHandler.Instance.saveData;
            textCoin.text = save.moneyAmount.ToString("N0");

            foreach (var item in upgradeItems)
            {
                var level = GetUpgradeLevel(item.upgradeType);
                var cost = GetUpgradeCost(item.upgradeType, level);
                var value = GetCurrentValueString(item.upgradeType, level);
                bool canUpgrade = save.moneyAmount >= cost;

                item.upgradeItem.UpdateUI(cost, value, canUpgrade);

                // Bind callback
                item.upgradeItem.OnUpgrade = () => OnClickUpgrade(item.upgradeType);
            }
        }

        public void OnClickUpgrade(PlayerUpgradeType upgradeType)
        {
            int level = GetUpgradeLevel(upgradeType);
            int cost = GetUpgradeCost(upgradeType, level);
            var save = SaveDataHandler.Instance.saveData;

            if (save.moneyAmount < cost)
                return;

            // Apply upgrade
            save.moneyAmount -= cost;
            SetUpgradeLevel(upgradeType, level + 1);
            SaveDataHandler.Instance.RequestSave();

            // Update UI
            RefreshUI();

            // Notify others
            SoArchitectureManager.Instance.OnMoneyChangedEvent.Raise();
        }
    }

    public enum PlayerUpgradeType
    {
        Capacity,
        Fuel,
        Speed,
        Range
    }
}
