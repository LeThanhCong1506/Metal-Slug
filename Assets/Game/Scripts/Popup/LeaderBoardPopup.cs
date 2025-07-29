using System.Collections.Generic;
using DenkKits.GameServices.SaveData;
using DenkKits.UIManager.Scripts.UIPopup;
using Game.Scripts.Popup.LeaderBoardComps;
using Sirenix.OdinInspector;
using SuperScrollView;
using UnityEngine;

namespace Game.Scripts.Popup
{
    public class LeaderBoardPopup : UIPopup
    {
        [Header("Reference")] [SerializeField] private LoopListView2 leaderBoardListView;
        [SerializeField] [ReadOnly] private List<LeaderItemData> loopListData;
        [SerializeField] [ReadOnly] private LeaderItemData userRank;
        [SerializeField] private LeaderBoardItem userLeaderBoardItem;
        [SerializeField] private GameObject leaderItemPrefab;

        [Header("Configuration")] [SerializeField]
        private bool useFakeData;

        [SerializeField] private LeaderItemFakeSo fakeDataItems;

        protected override void OnInit()
        {
            base.OnInit();
            leaderBoardListView.InitListView(loopListData.Count, OnItemListViewLoaded);
        }

        protected override void OnShowing()
        {
            base.OnShowing();
            LoadLeaderBoardData();
        }

        [Button("Reload LeaderBoard")]
        public void LoadLeaderBoardData()
        {
            userRank = new LeaderItemData()
            {
                userName = "You",
                score = SaveDataHandler.Instance.UserHighScore,
            };
            loopListData = new List<LeaderItemData>();
            foreach (LeaderItemData data in fakeDataItems.dataItems)
            {
                loopListData.Add(data);
            }

            loopListData.Add(userRank);
            loopListData.Sort((x, y) => y.score.CompareTo(x.score));
            userLeaderBoardItem.InitItem(loopListData.IndexOf(userRank) + 1, "YOU", userRank.score);
            leaderBoardListView.RecycleAllItem();
            leaderBoardListView.SetListItemCount(loopListData.Count);
            leaderBoardListView.MovePanelToItemIndex(0, 0);
        }

        private LoopListViewItem2 OnItemListViewLoaded(LoopListView2 view, int index)
        {
            var mList = loopListData;
            if (index < 0 || mList.Count <= index) return null;
            var data = mList[index];
            var itemObj = view.NewListViewItem(leaderItemPrefab.name);
            var itemUI = itemObj.GetComponent<LeaderBoardItem>();

            itemUI.InitItem(index + 1, data.userName, data.score);
            return itemObj;
        }

    }
}