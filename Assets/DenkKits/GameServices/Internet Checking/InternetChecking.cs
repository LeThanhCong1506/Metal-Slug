using System;
using Imba.Utils;
using UnityEngine;

namespace DenkKits.GameServices.Internet_Checking
{
    public class InternetChecking : AutoSingletonMono<InternetChecking>
    {
        private DateTime _currentTimeCheckInternet;

        public override void Awake()
        {
            base.Awake();
            Init();
        }

        public void Init()
        {
            CheckInternet();
            ResetTimeCheckInternet();
        }

        void Update()
        {
            if ((DateTime.Now - _currentTimeCheckInternet).TotalSeconds >= GameSettings.Instance.timeCheckInternet)
            {
                CheckInternet();
                ResetTimeCheckInternet();
            }
        }

        private void CheckInternet()
        {
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                // CONNECTED
                Time.timeScale = 1;
            }
            else
            {
                // SHOW LOST CONNECTION
                Time.timeScale = 0;
            }
        }

        private void ResetTimeCheckInternet()
        {
            _currentTimeCheckInternet = DateTime.Now;
        }
    }
}