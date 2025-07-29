using System.Collections.Generic;
using DenkKits.GameServices.Audio.Scripts;
using DenkKits.UIManager.Scripts.UIPopup;
using Game.Scripts.Popup.LeaderBoardComps;
using Sirenix.OdinInspector;
using SuperScrollView;
using UnityEngine;

namespace Game.Scripts.Popup
{
    public class TaskPopup : UIPopup
    {
        [Header("Reference")] [SerializeField] private LoopListView2 leaderBoardListView;
        [SerializeField] [ReadOnly] private List<LeaderItemData> loopListData;
        [SerializeField] [ReadOnly] private LeaderItemData userRank;
        [SerializeField] private LeaderBoardItem userLeaderBoardItem;
        [SerializeField] private GameObject leaderItemPrefab;

        [Header("Configuration")] [SerializeField]
        private bool useFakeData;

        [SerializeField] private LeaderItemFakeSo fakeDataItems;


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
                score = 1000,
            };
            loopListData = fakeDataItems.dataItems;
            leaderBoardListView.RecycleAllItem();
            leaderBoardListView.InitListView(loopListData.Count, OnItemListViewLoaded);
            // leaderBoardListView.MovePanelToItemIndex(0,0);
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

        public override void Hide(bool instantHide = false)
        {
            base.Hide(instantHide);
            AudioManager.Instance.PlaySfx(AudioName.UI_Click);
        }
    }
}