
using DenkKits.UIManager.Scripts.UIAlert;
using DenkKits.UIManager.Scripts.UIPopup;
using DenkKits.UIManager.Scripts.UIView;
using DG.Tweening;
using Imba.UI;
using Imba.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace DenkKits.UIManager.Scripts.Base
{
    /// <summary>
    /// UIManager: Manage all UI element include Popups, Views, Notices, Alert ...
    /// </summary>
    public class UIManager : ManualSingletonMono<UIManager>
    {
        public Camera         UICamera;
        public UIViewManager  ViewManager;
        public UIPopupManager PopupManager;
        public UIAlertManager AlertManager;

        public RectTransform CanvasRect;

        [Header("LOADING")]
        public RectTransform loadingObject;

        [Header("Transition")]
        public RectTransform transitionRect;

        public RectTransform transitionRender;

        public bool IsShowingLoading
        {
            get
            {
                if (loadingObject)
                {
                    return loadingObject.gameObject.activeSelf;
                }

                return false;
            }
        }

        public override void Awake()
        {
            base.Awake();
            if (!ViewManager)
            {
                ViewManager = GetComponentInChildren<UIViewManager>();
            }

            if (!PopupManager)
            {
                PopupManager = GetComponentInChildren<UIPopupManager>();
            }

            if (!AlertManager)
            {
                AlertManager = GetComponentInChildren<UIAlertManager>();
            }

            if (loadingObject)
            {
                loadingObject.gameObject.SetActive(false);
            }

            // mainCanvas.referenceResolution = new Vector2(Screen.width, Screen.height);


#if DISABLE_LOG
			Debug.unityLogger.filterLogType = LogType.Error;
#endif
        }

        void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
        }

        public void ShowLoading(float timeToHide = 0)
        {
            //Debug.Log ("ShowLoading");
            //DoLoadingAnimation();
            loadingObject.gameObject.SetActive(true);
            if (timeToHide <= 0) CancelInvoke("HideLoadingCallback");
            else Invoke("HideLoadingCallback", timeToHide);
        }

        public void HideLoading()
        {
            CancelInvoke("HideLoadingCallback");
            loadingObject.gameObject.SetActive(false);
        }

        public void ShowTransition(UnityAction onDone)
        {
            transitionRect.SetActive(true);
            transitionRender.localScale = Vector3.zero;
            transitionRender.DOScale(Vector3.one * 9, 0.5f).OnComplete(() => { onDone?.Invoke(); })
                .SetEase(Ease.Linear);
        }

        public void HideTransition(UnityAction onDone)
        {
            transitionRender.DOScale(Vector3.zero, 0.5f).OnComplete(() =>
            {
                transitionRect.SetActive(false);
                onDone?.Invoke();
            }).SetEase(Ease.Linear);
        }

        void HideLoadingCallback()
        {
            loadingObject.gameObject.SetActive(false);
        }

        public bool ShowDebugLog = true;

        public static void DebugLog<T>(string message, T com)
        {
#if UNITY_EDITOR
            if (!Instance || !Instance.ShowDebugLog) return;

            string msg = string.Format("[{0}] {1}", com != null ? com.GetType().ToString() : "", message);
            Debug.Log(string.Format("<color=blue>[UIManager][{0}] {1}</color>", com.GetType(), message));
#endif
        }
    }
}