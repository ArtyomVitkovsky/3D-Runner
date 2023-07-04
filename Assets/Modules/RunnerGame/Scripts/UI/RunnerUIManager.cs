using System;
using System.Collections.Generic;
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
        [SerializeField] private WinScreen.WinScreen winScreen;

        public UnityAction OnTapToStart;
    
        private UIManager uiManager;

        public PlayerStatsScreen PlayerStatsScreen => playerStatsScreen;

        [Inject]
        private void Construct(UIManager uiManager)
        {
            this.uiManager = uiManager;
            this.uiManager.AddScreen(tapToStartScreen);
            this.uiManager.AddScreen(playerStatsScreen);
            this.uiManager.AddScreen(winScreen);
            
            tapToStartScreen.Initialize();
            playerStatsScreen.Initialize();
            winScreen.Initialize();
        
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
        
        public void ShowWinScreen(Dictionary<PlatformType, int> passedPlatforms)
        {
            winScreen.SetPassedPlatforms(passedPlatforms);
            uiManager.SetScreenActive<WinScreen.WinScreen>(true, false);
        }

        private void OnDestroy()
        {
            uiManager.RemoveScreen(tapToStartScreen);
            uiManager.RemoveScreen(playerStatsScreen);
            uiManager.RemoveScreen(winScreen);
        }
    }
}