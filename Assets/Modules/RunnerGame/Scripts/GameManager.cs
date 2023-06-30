using System.Collections.Generic;
using Modules.MainModule.Scripts.UI;
using Modules.MainModule.Scripts.UI.Screens;
using Modules.RunnerGame.Scripts.Level;
using Modules.RunnerGame.Scripts.Player;
using Modules.RunnerGame.Scripts.UI;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlatformConfigs platformConfigs;
    private PlatformGenerator platformGenerator;

    private Dictionary<PlatformType, int> passedPlatforms;

    private UIManager uiManager;
    private RunnerUIManager runnerUIManager;
    private Player player;
    [Inject]
    private void Construct(UIManager uiManager, RunnerUIManager runnerUIManager, Player player)
    {
        GameSettings.IS_PAUSED = true;

        this.uiManager = uiManager;
        this.runnerUIManager = runnerUIManager;
        runnerUIManager.OnTapToStart += StartGame;
        
        this.player = player;
        
        SetupUI();

        passedPlatforms = new Dictionary<PlatformType, int>();
        platformGenerator = new PlatformGenerator(platformConfigs, 50, player, transform);
        platformGenerator.OnPlatformPassed += OnPlatformPassed;
    }

    private void OnPlatformPassed(PlatformType type)
    {
        if (!passedPlatforms.ContainsKey(type))
        {
            passedPlatforms.Add(type, 0);
        }

        passedPlatforms[type]++;
    }

    private void SetupUI()
    {
        player.OnSpeedChange += runnerUIManager.PlayerStatsScreen.UpdateSpeedText;
        player.OnHealthChange += runnerUIManager.PlayerStatsScreen.UpdateHealthText;
        runnerUIManager.PlayerStatsScreen.UpdateSpeedText(player.Speed);
        runnerUIManager.PlayerStatsScreen.UpdateHealthText(player.Health);
    }

    private void StartGame()
    {
        GameSettings.IS_PAUSED = false;
    }

    private void Start()
    {
        platformGenerator.OnPlatformGenerated += OnPlatformGenerated;
        platformGenerator.OnGenerationFinished += OnGenerationFinished;
        platformGenerator.Generate();
    }
    
    private void Update()
    {
        platformGenerator.Update();
    }

    private void OnPlatformGenerated(float progress)
    {
        uiManager.GetScreen<LoadingScreen>().SetLoadingProgress(progress);
    }

    private void OnGenerationFinished()
    {
        uiManager.SetScreenActive<LoadingScreen>(false, false);
        runnerUIManager.ShowTapToStartScreen();
    }
}