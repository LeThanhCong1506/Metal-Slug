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
        [SerializeField] private bool testStat;

        private GameView _gameView;
        private Joystick _joystickMovement;

        private bool _isGamePaused;
        private int _userScore;
        private float _remainingTime;
        private bool _isGameEnd;

        #region Unity Methods

        private void Start()
        {
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
                AudioManager.Instance.PlaySfx(AudioName.Gameplay_ChangeElemet);
            });
        }

        private void Update()
        {
            if (_isGamePaused) return;
            if (_isGameEnd) return;

            HandleJoystickInput();
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
            _joystickMovement = _gameView.Joystick;
        }


        private void InitPlayerStatsTest()
        {
        }

        private void InitPlayerStats()
        {
        }

        private void InitGameEnvironment()
        {
        }

        #endregion

        #region Input Handling

        private void HandleJoystickInput()
        {
            if (!_joystickMovement.isHolding) return;

            float forwardInput = _joystickMovement.Vertical;
            float turnInput = _joystickMovement.Horizontal;
        }

        private void HandleDebugInput()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                ShowEndGame();
            }
        }

        #endregion


        #region Game State

        public void PauseGame() => _isGamePaused = true;
        public void ResumeGame() => _isGamePaused = false;

        public void ShowEndGame()
        {
            if (_isGameEnd) return;
            _isGameEnd = true;

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