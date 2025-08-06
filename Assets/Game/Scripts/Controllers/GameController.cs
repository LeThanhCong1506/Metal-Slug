using System.Collections.Generic;
using DenkKits.GameServices.Audio.Scripts;
using DenkKits.GameServices.Manager;
using DenkKits.GameServices.SaveData;
using DenkKits.UIManager.Scripts.Base;
using DenkKits.UIManager.Scripts.UIPopup;
using DenkKits.UIManager.Scripts.UIView;
using Game.Scripts.Popup;
using Game.Scripts.Views;
using Imba.Utils;
using UnityEngine;

namespace Game.Scripts.Controllers
{
    public class GameController : ManualSingletonMono<GameController>
    {
        [SerializeField] private Transform playerTransform;
        //[SerializeField] private Player player;
        [SerializeField] private List<GameObject> levelList;

        [SerializeField] private bool testStat;

        private GameView _gameView;
        private Joystick _joystickMovement;

        private bool _isGamePaused;
        private int _userScore;
        private int _userHealth = 3;
        private int _points = 3;
        private float _remainingTime;
        private bool _isGameEnd;
        private GameObject _currentLevel;

        #region Unity Methods

        private void Start()
        {
            _userHealth = 3;
            _points = 0;
            RegisterEvents();
            InitGameView();
            if (testStat)
            {
                InitPlayerStatsTest();
            }
            else
            {
                InitPlayerStats();
            }

            InitGameEnvironment();
            UIManager.Instance.HideTransition(() =>
            {
                // AudioManager.Instance.PlaySfx(AudioName.Gameplay_ChangeElemet);
            });
        }

        private void Update()
        {
            if (_isGamePaused) return;
            if (_isGameEnd) return;

            //HandleJoystickInput();
            HandleKeyboardInput();
#if UNITY_EDITOR
            HandleDebugInput();
#endif
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            UnregisterEvents();
        }

        #endregion

        #region Initialization

        private void InitGameView()
        {
            _gameView = UIManager.Instance.ViewManager.GetViewByName<GameView>(UIViewName.GameView);
            UIManager.Instance.ViewManager.ShowView(UIViewName.GameView);
            // ADDITION HERE
            //_gameView.SetHealth(_userHealth);
            //_gameView.SetApple(_apple);
        }


        private void InitPlayerStatsTest()
        {
        }

        private void InitPlayerStats()
        {
        }


        private void InitGameEnvironment()
        {
            int curLevel = SaveDataHandler.Instance.saveData.currentLevelIndex;
            if (curLevel < 0 || curLevel >= levelList.Count)
            {
                Debug.LogError("Invalid level index");
                return;
            }

            _currentLevel = Instantiate(levelList[curLevel]);

            // Lấy component Map trong level để truy cập startPoint
            var map = _currentLevel.GetComponent<Map>();
            if (map != null && map.startPoint != null)
            {
                playerTransform.position = map.startPoint.position;
            }
            else
            {
                Debug.LogWarning("Map or startPoint not set in level prefab.");
            }
        }

        #endregion

        #region Input Handling

        private void HandleKeyboardInput()
        {
            float horizontal = Input.GetAxisRaw("Horizontal"); // A/D hoặc ←/→
            Vector2 input = new Vector2(horizontal, 0f);
            //player.Move(input);

            // Nhảy với Space hoặc W
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
            {
                //player.Jump();
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                float currentTime = Time.time;
                //if (currentTime - _lastShootTime > ShootCooldown && _apple > 0)
                //{
                //    _apple--;
                //    _lastShootTime = currentTime;

                //    player.Shoot();
                //    _gameView.SetApple(_apple);
                //    AudioManager.Instance.PlaySfx(AudioName.Gameplay_LootElemet);
                //}
            }

        }

        private float _lastShootTime = -1f;
        private const float ShootCooldown = 0.5f;

        //private void HandleJoystickInput()
        //{
        //    float currentTime = Time.time;

        //    bool shootByRT = Input.GetAxisRaw("RT") > 0.8f;
        //    bool shootByLT = Input.GetAxis("LT") > 0.8f;
        //    bool canShoot = (shootByLT || shootByRT) && currentTime - _lastShootTime > ShootCooldown;

        //    //if (canShoot && _apple > 0)
        //    //{
        //    //    _apple--;
        //    //    _lastShootTime = currentTime;

        //    //    player.Shoot();
        //    //    _gameView.SetApple(_apple);
        //    //    AudioManager.Instance.PlaySfx(AudioName.Gameplay_LootElemet);
        //    //}

        //    //if (Input.GetKeyDown(KeyCode.JoystickButton0))
        //    //{
        //    //    player.Jump();
        //    //}
        //}


        private void HandleDebugInput()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                ShowEndGame();
            }
        }

        #endregion

        #region LOGIC

        public void EarnCoin()
        {
            _userScore++;
            AudioManager.Instance.PlaySfx(AudioName.UI_Wind);
            //_gameView.SetCoin(_userScore);
        }

        public void EarnApple()
        {
            //_apple++;
            AudioManager.Instance.PlaySfx(AudioName.UI_Wind);
            //_gameView.SetApple(_apple);
        }

        public void TakeDamage(int i)
        {
            _userHealth--;
            if (_userHealth == 0)
            {
                ShowFailGame();
            }

            //_gameView.SetHealth(_userHealth);
        }

        #endregion

        #region Game State

        public void PauseGame()
        {
            //player.Stop();
            _isGamePaused = true;
        }

        public void ResumeGame() => _isGamePaused = false;

        public void ShowFailGame()
        {
            if (_isGameEnd) return;
            _isGameEnd = true;
            PauseGame();

            AudioManager.Instance.PlaySfx(AudioName.Gameplay_EnemyHit);
            //UIManager.Instance.PopupManager.ShowPopup(UIPopupName.FailPopup);
        }

        public void ShowEndGame()
        {
            if (_isGameEnd) return;
            _isGameEnd = true;

            PauseGame();
            var starEarn = _userHealth;
            AudioManager.Instance.PlaySfx(AudioName.Gameplay_EndGame);

            var level = SaveDataHandler.Instance.saveData.currentLevelIndex;
            level++;
            int currentUnlockedLevel = SaveDataHandler.Instance.saveData.level;

            if (level == currentUnlockedLevel)
            {
                SaveDataHandler.Instance.saveData.level++;
            }

            //SaveDataHandler.Instance.SaveStarForLevel(level, starEarn);
            SaveDataHandler.Instance.RequestSave();


            var param = new EndGamePopupParam
            {
                //starWin = starEarn,
                //coinWin = _userScore,
                //bonus = false
            };

            UIManager.Instance.PopupManager.ShowPopup(UIPopupName.EndGamePopup, param);
        }

        public void ShowEndGame(string reason = "Time's up")
        {
            if (_isGameEnd) return;
            _isGameEnd = true;

            AudioManager.Instance.PlaySfx(AudioName.Gameplay_EndGame);
            PauseGame();

            var param = new EndGamePopupParam
            {
            };

            if (_userScore > SaveDataHandler.Instance.UserHighScore)
            {
                SaveDataHandler.Instance.UserHighScore = _userScore;
                SaveDataHandler.Instance.RequestSave();
            }

            UIManager.Instance.PopupManager.ShowPopup(UIPopupName.EndGamePopup, param);
        }

        #endregion

        #region Event Registration

        private void RegisterEvents()
        {
            var manager = SoArchitectureManager.Instance;
            manager.PauseGame.AddListener(PauseGame);
            manager.ResumeGame.AddListener(ResumeGame);
        }

        private void UnregisterEvents()
        {
            var manager = SoArchitectureManager.Instance;
            manager.PauseGame.RemoveListener(PauseGame);
            manager.ResumeGame.RemoveListener(ResumeGame);
        }

        #endregion
    }
}