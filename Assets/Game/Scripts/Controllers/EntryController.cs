using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.Scripts.Controllers
{
    public class EntryController : MonoBehaviour
    {
        [SerializeField] private GameObject manager;
        [SerializeField] private Slider slider;
        [SerializeField] private TextMeshProUGUI textPercenst;
        [SerializeField] private float loadDuration = 2f;

        private void Awake()
        {
            // Set the target frame rate
            Application.targetFrameRate = 60;
            loadDuration = Random.Range(2, 3);
            DontDestroyOnLoad(manager);
        }

        private void Start()
        {
            slider.value = 0f;
            slider.DOValue(100f, loadDuration)
                .SetEase(Ease.OutCubic)
                .OnComplete(() => { SceneManager.LoadScene(GameConstants.SceneMain); }).OnUpdate(
                    () => { textPercenst.text ="Loading..." + Mathf.RoundToInt(slider.value )  + "%"; } );
        }
    }
}