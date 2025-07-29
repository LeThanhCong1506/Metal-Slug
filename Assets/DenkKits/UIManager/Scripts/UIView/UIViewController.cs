
using DenkKits.UIManager.Scripts.UIAnimation;
using Imba.UI;
using UnityEngine;
using UnityEngine.UI;

namespace DenkKits.UIManager.Scripts.UIView
{
    
    /// <summary>
    /// Component to control the view
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(GraphicRaycaster))]
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class UIViewController : MonoBehaviour
    {
        #region Constants

        #endregion

        #region Static Fields
        #endregion

        #region Events
        #endregion

        #region Public Vars

        public UIViewName ViewName;
        public bool CanGoBack = true;
        //Determine if this view can hide when click Back Button
        //public bool HideByBackButton = true;

        public bool DeactiveGameObjectWhenHide = false;
        
        public UIViewBehavior ShowBehavior = new UIViewBehavior(AnimationType.Show);
	    
        public UIViewBehavior HideBehavior = new UIViewBehavior(AnimationType.Hide);

        #endregion

        #region Private Vars

        private UIViewManager _viewManager; //referrence to view controller
        private UIView _view;//referrence to view
        
        #endregion

        #region Properties


        public UIView View
        {
            get
            {
                if (!_view)
                    _view = GetComponent<UIView>();
                return _view;
            }
        }

        public UIViewManager ViewManager => _viewManager;
        

        #endregion

        #region Constructors
        #endregion

        #region Unity Methods
        #endregion

        #region Public Methods

        public void Initialize(UIViewManager viewManager)
        {
            _viewManager = viewManager;
        }

        public void Show(object ps = null, bool isBack = false)
        {
            View.Show(ps, isBack);
        }

        public void Hide(bool instantHide = false)
        {
            View.Hide(instantHide);
        }

        public void DoDestroy()
        {
            if (View != null)
            {
                Debug.LogWarning("Destroy " + ViewName);
                Destroy(View.gameObject);
            }
        }

        #endregion

        #region Private Methods
        #endregion

        #region Virtual Methods
        #endregion

        #region Static Methods
        #endregion
    }
}