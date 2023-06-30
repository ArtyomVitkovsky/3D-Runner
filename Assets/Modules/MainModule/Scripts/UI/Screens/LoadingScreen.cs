using Modules.MainModule.Scripts.UI.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.MainModule.Scripts.UI.Screens
{
    public class LoadingScreen : MonoBehaviour, IScreen
    {
        [SerializeField] private Image loadingProgressBar;
        public void Initialize()
        {
            
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void SetLoadingProgress(float progress)
        {
            loadingProgressBar.fillAmount = progress;
        }
    }
}