using System;
using DenkKits.GameServices.Manager;
using Imba.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DenkKits.GameServices.SaveData
{
    public class SaveDataHandler : ManualSingletonMono<SaveDataHandler>
    {
        public SaveData saveData;

        #region Private Variables

        private const string SaveKey = "UserDataKey";
        private bool _requestSave;

        #endregion

        public int UserHighScore
        {
            get => saveData.userHighScore;
            set => saveData.userHighScore = value;
        }

        public int UserHighScoreTime
        {
            get => saveData.userHighScoreTime;
            set => saveData.userHighScoreTime = value;
        }

        #region DAILY REWARD

        [Button]
        public void HackCoin()
        {
            saveData.moneyAmount += 500;
            RequestSave();
            SoArchitectureManager.Instance.OnMoneyChangedEvent.Raise();
        }

        [Button]
        public void ResetLastTimeEarnedDailyReward()
        {
            saveData.lastTimeEarnedDailyReward = DateTime.Now.ToString();
        }

        public void AddDayDailyRewardClaimed(int day)
        {
            if (!saveData.dailyRewardClaimedList.Contains(day))
            {
                saveData.dailyRewardClaimedList.Add(day);
            }
        }

        public void ResetDailyRewardList()
        {
            saveData.dailyRewardClaimedList.Clear();
        }

        public bool IsDayDailyRewardClaimed(int day)
        {
            if (saveData.dailyRewardClaimedList.Count == 0)
            {
                return false;
            }

            for (int i = 0; i < saveData.dailyRewardClaimedList.Count; i++)
            {
                if (saveData.dailyRewardClaimedList[i] == day)
                {
                    return true;
                }
            }

            return false;
        }

        public DateTime GetLastTimeEarnedDailyReward()
        {
            return DateTime.Parse(saveData.lastTimeEarnedDailyReward);
        }

        #endregion

        #region Unity Methods

        public override void Awake()
        {
            base.Awake();
            OnLoadData();
        }

        private void OnDisable()
        {
            OnSaveData();
        }

        private void OnApplicationPause(bool pause)
        {
            OnSaveData();
        }

        private void OnApplicationQuit()
        {
            OnSaveData();
        }

        private void Update()
        {
            if (_requestSave)
            {
                _requestSave = false;
                OnSaveData();
            }
        }

        #endregion

        #region Private Methods

        private string RandomName()
        {
            int random = Random.Range(0, 10000);
            return "Player" + random;
        }

        private void OnSaveData()
        {
            // SaveDataByES3();
            SaveDataByPlayerPrefs();
        }

        private void OnLoadData()
        {
            // LoadDataByES3();
            LoadDataByPlayerPrefs();
        }

        private void LoadDataByPlayerPrefs()
        {
            var json = PlayerPrefs.GetString(SaveKey, "");
            if (!PlayerPrefs.HasKey(SaveKey))
            {
                saveData = new SaveData();
            }
            else
            {
                var loadUserData = JsonUtility.FromJson<SaveData>(json);
                saveData = loadUserData;
            }
        }

        private void SaveDataByPlayerPrefs()
        {
            string json = JsonUtility.ToJson(saveData);
            PlayerPrefs.SetString(SaveKey, json);
            PlayerPrefs.Save();
        }

        #endregion

        #region Public Method

        public void RequestSave()
        {
            _requestSave = true;
            Debug.Log("Requesting save data");
        }

        #endregion
    }
}