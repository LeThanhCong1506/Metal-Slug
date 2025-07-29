using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Scripts.Popup.UpgradeComps
{
    public class UpgradeItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textPrice;
        [SerializeField] private TextMeshProUGUI textCurrentValue;
        [SerializeField] private Button buttonUpgrade;
        [SerializeField] private GameObject notiCanUpgrade;

        public UnityAction OnUpgrade;

        public void UpdateUI(int price, string currentValue, bool canUpgrade)
        {
            textPrice.text = price.ToString();
            textCurrentValue.text = "Current: " + currentValue;
            buttonUpgrade.interactable = canUpgrade;
            notiCanUpgrade.SetActive(canUpgrade);
        }

        public void Upgrade()
        {
            OnUpgrade?.Invoke();
        }
    }
}