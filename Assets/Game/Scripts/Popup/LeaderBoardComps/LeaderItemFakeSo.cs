using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Scripts.Popup.LeaderBoardComps
{
    [Serializable]
    public class LeaderItemData
    {
        public int id;
        public string userName;
        public int score;
    }
    [CreateAssetMenu(fileName = "LeaderItemFakeSo", menuName = "DenkKits/Sample/Create Leader Item Fake Data")]
    public class LeaderItemFakeSo : ScriptableObject
    {
        public List<LeaderItemData> dataItems;

        [Button]
        public void ShortListData()
        {
            dataItems.Sort((x, y) => y.score.CompareTo(x.score));
        }
    }
}