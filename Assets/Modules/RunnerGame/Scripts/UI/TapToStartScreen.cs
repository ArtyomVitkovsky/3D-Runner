using Modules.MainModule.Scripts.UI.Interfaces;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Modules.RunnerGame.Scripts.UI
{
    public class TapToStartScreen : MonoBehaviour, IScreen
    {
        [SerializeField] private Button startButton;

        public UnityAction OnTapToStart;
    
        public void Initialize()
        {
            startButton.onClick.AddListener(OnTapToStartHandler);
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void OnTapToStartHandler()
        {
            OnTapToStart?.Invoke();
        }
    }
}