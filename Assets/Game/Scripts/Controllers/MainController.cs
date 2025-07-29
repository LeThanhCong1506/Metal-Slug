using DenkKits.UIManager.Scripts.Base;
using DenkKits.UIManager.Scripts.UIView;
using Game.Scripts.Views;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts.Controllers
{
    public class MainController : MonoBehaviour
    {
        [ReadOnly] public MainView mainView;

        private void Start()
        {
            UIManager.Instance.ViewManager.ShowView(UIViewName.MainView);
            mainView = UIManager.Instance.ViewManager.GetViewByName<MainView>(UIViewName.MainView);

            // Bind callbacks
            mainView.onLoadGame = OnLoadGame;
            mainView.onClickShowSomeThing = ShowPreviewBoat;
            mainView.onClickBuySomethingSuccess = ConfirmBoatPurchase;
            mainView.onClickBackToMain = BackToCurrentBoat;
            UIManager.Instance.HideTransition(() =>
            {
                
            });
        }



        private void ShowPreviewBoat()
        {
        
        }

        private void ConfirmBoatPurchase()
        {
         
        }

        private void BackToCurrentBoat()
        {

        }
        private void OnLoadGame()
        {
            UIManager.Instance.ShowTransition(() => { SceneManager.LoadScene(GameConstants.SceneGame); });
        }
    }
}
