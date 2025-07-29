using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace DenkKits.GameServices.SaveData
{
    [Serializable]
    public class SaveData
    {
        [Title("Strings")] public string profileName = "";
        public int playerRegion;
        public string userID = "";
        public string lastTimeEarnedDailyReward = DateTime.Now.ToString();
        public string lastDateMonthlyChallenge = DateTime.Now.ToString();
        public string rankingCountdownTime;

        [Title("Tutorials")]
        public bool tapPlayTutorial;

        [Title("Numbers")]
        public int level = 1;
        public int currentLevelIndex = 1;
        public int moneyAmount;
        public int currentDayDailyReward = 1;
        public int userHighScore = 0;
        public int gameModeChoose = 0;
        public int currentBoatId = 0;
        
        public int userHighScoreTime = -1;

        [Title("Booleans")]
        public bool isShowRateUs;

        [Title("Lists")] 
        public List<int> dailyRewardClaimedList = new();
        
        [Title("Boat Upgrades")]
        public int speedLevel = 0;
        public int capacityLevel = 0;
        public int fuelLevel = 0;
        public int suctionRangeLevel = 0;
        public bool isRadarUnlocked = false;

    }
}