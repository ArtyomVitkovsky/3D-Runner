using System;
using System.Collections.Generic;
using Modules.MainModule.Scripts.UI;
using Modules.RunnerGame.Scripts.UI.WinScreen;
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
        [SerializeField] private LooseScreen.LooseScreen looseScreen;

        public UnityAction OnTapToStart;
        public UnityAction OnContinueAfterLoose;

        private UIManager uiManager;

        public PlayerStatsScreen PlayerStatsScreen => playerStatsScreen;
        
        
        [Inject]
        private void Construct(UIManager uiManager)
        {
            this.uiManager = uiManager;
            this.uiManager.AddScreen(tapToStartScreen);
            this.uiManager.AddScreen(playerStatsScreen);
            this.uiManager.AddScreen(winScreen);
            this.uiManager.AddScreen(looseScreen);

            tapToStartScreen.Initialize();
            playerStatsScreen.Initialize();
            winScreen.Initialize();
            looseScreen.Initialize();

            tapToStartScreen.OnTapToStart += OnTapToStartHandler;
            looseScreen.OnContinue += OnContinueAfterLooseHandler;

            canvas.worldCamera = uiManager.Camera;
        }

        private void OnContinueAfterLooseHandler()
        {
            OnContinueAfterLoose?.Invoke();
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

        public void HideWinScreen()
        {
            uiManager.SetScreenActive<WinScreen.WinScreen>(false, false);
        }

        public void ShowLooseScreen(Dictionary<PlatformType, int> passedPlatforms)
        {
            looseScreen.SetPassedPlatforms(passedPlatforms);
            uiManager.SetScreenActive<LooseScreen.LooseScreen>(true, false);
        }

        public void HideLooseScreen()
        {
            uiManager.SetScreenActive<LooseScreen.LooseScreen>(false, false);
        }

        private void OnDestroy()
        {
            uiManager.RemoveScreen(tapToStartScreen);
            uiManager.RemoveScreen(playerStatsScreen);
            uiManager.RemoveScreen(winScreen);
            uiManager.RemoveScreen(looseScreen);
        }
    }
}