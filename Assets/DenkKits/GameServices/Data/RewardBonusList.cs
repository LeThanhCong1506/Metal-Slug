using System;
using System.Collections.Generic;

namespace DenkKits.GameServices.Data
{
    [Serializable]
    public class RewardBonusList
    {
        public List<RewardBonusData> rewardList;
    }

    [Serializable]
    public class RewardBonusData
    {
        public AuxiliaryType type;
        public int amount;
    }
}