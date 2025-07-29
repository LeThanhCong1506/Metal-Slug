using Imba.Utils;
using UnityEngine;

namespace DenkKits.GameServices
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "ResourceConfig/GameSettings")]
    public class GameSettings : AutoSingletonMono<GameSettings>
    {
        public bool isShowTutorial;
        public bool isUnlimitedMoney;
        public bool unlockedFullOfLevels;

        [Space] public float crossFadeButtonDuration = 0.2f;
        public float timeCheckInternet = 2;
    }
}