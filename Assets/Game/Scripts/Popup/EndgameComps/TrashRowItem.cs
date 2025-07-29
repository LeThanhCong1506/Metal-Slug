using TMPro;
using UnityEngine;

namespace Game.Scripts.Popup.EndgameComps
{
    public class TrashRowItem : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI textMeshNameAndQuantity;
        [SerializeField] private TextMeshProUGUI TextTotalKg;
        [SerializeField] private TextMeshProUGUI textTotalEarnings;

        public TextMeshProUGUI TextMeshNameAndQuantity => textMeshNameAndQuantity;

        public TextMeshProUGUI TextTotalKg1 => TextTotalKg;

        public TextMeshProUGUI TextTotalEarnings => textTotalEarnings;
    }
}
