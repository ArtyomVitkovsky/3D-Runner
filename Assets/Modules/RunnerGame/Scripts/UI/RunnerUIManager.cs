using Modules.MainModule.Scripts.UI;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Modules.RunnerGame.Scripts.UI
{
    public class RunnerUIManager : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private TapToStartScreen tapToStartScreen;
        [SerializeField] private PlayerStatsScreen playerStatsScreen;

        public UnityAction OnTapToStart;
    
        private UIManager uiManager;

        public PlayerStatsScreen PlayerStatsScreen => playerStatsScreen;

        [Inject]
        private void Construct(UIManager uiManager)
        {
            this.uiManager = uiManager;
            uiManager.AddScreen(tapToStartScreen);
            uiManager.AddScreen(playerStatsScreen);
            
            tapToStartScreen.Initialize();
            playerStatsScreen.Initialize();
        
            tapToStartScreen.OnTapToStart += OnTapToStartHandler;

            canvas.worldCamera = uiManager.Camera;
        }

        private void OnTapToStartHandler()
        {
            OnTapToStart?.Invoke();
            uiManager.SetScreenActive<PlayerStatsScreen>(true, false);
        }

        public void ShowTapToStartScreen()
        {
            uiManager.SetScreenActive<TapToStartScreen>(true, false);
        }
    }
}