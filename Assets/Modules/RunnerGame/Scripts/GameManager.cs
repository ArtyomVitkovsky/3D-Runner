using System;
using System.Collections.Generic;
using Modules.MainModule.Scripts.UI;
using Modules.MainModule.Scripts.UI.Screens;
using Modules.RunnerGame.Scripts.Level;
using Modules.RunnerGame.Scripts.Level.Buff;
using Modules.RunnerGame.Scripts.Level.Platform;
using Modules.RunnerGame.Scripts.Player;
using Modules.RunnerGame.Scripts.Setup;
using Modules.RunnerGame.Scripts.UI;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlatformConfigs platformConfigs;
    [SerializeField] private BuffConfigs buffConfigs;

    private PlatformGenerator platformGenerator;
    private PlatformsSystem platformsSystem;

    private BuffsGenerator buffsGenerator;

    private Dictionary<PlatformType, int> passedPlatforms;
    private List<BuffObject> buffObjects;

    private UIManager uiManager;
    private RunnerUIManager runnerUIManager;
    private Player player;
    private Platform restartPlatform;

    [Inject]
    private void Construct(UIManager uiManager, RunnerUIManager runnerUIManager, Player player)
    {
        GameSettings.IS_PAUSED = true;

        SetupPlayer(player);

        SetupUI(uiManager, runnerUIManager);

        SetupPlatformGenerator();
        SetupBuffGenerator(platformGenerator.Platforms[PlatformType.Default]);
    }

    private void SetupPlayer(Player player)
    {
        this.player = player;
        this.player.OnDeath += OnPlayerDeath;
    }

    private void OnPlayerDeath(Platform loosePlatform)
    {
        restartPlatform = loosePlatform;
        bool isRestartPlatformSetted = false;
        
        var targetIndex = loosePlatform.Index;
        
        while (!isRestartPlatformSetted)
        {
            foreach (var platform in platformGenerator.Platforms[PlatformType.Default])
            {
                if (platform.Index == targetIndex)
                {
                    restartPlatform = platform;
                    isRestartPlatformSetted = true;
                    break;
                }
            }

            targetIndex++;
        }
       

        GameSettings.IS_PAUSED = true;
        runnerUIManager.ShowLooseScreen(passedPlatforms);
    }

    private void OnContinueAfterLoose()
    {
        player.transform.position = restartPlatform.Transform.position;
        player.Reset();

        runnerUIManager.HideLooseScreen();
        runnerUIManager.ShowTapToStartScreen();
    }

    private void SetupPlatformGenerator()
    {
        passedPlatforms = new Dictionary<PlatformType, int>();
        platformGenerator = new PlatformGenerator(platformConfigs, 50, player, transform);

        platformGenerator.OnPlatformGenerated += OnPlatformGenerated;
        platformGenerator.OnGenerationFinished += OnGenerationFinished;
        platformGenerator.Generate();

        platformsSystem =
            new PlatformsSystem(
                platformGenerator.Platforms,
                player,
                platformGenerator.PlatformsCount);

        platformsSystem.OnPlatformPassed += OnPlatformPassed;
    }

    private void SetupBuffGenerator(List<Platform> generatedDefaultPlatforms)
    {
        buffObjects = new List<BuffObject>();
        buffsGenerator = new BuffsGenerator(buffConfigs, platformGenerator.Platforms[PlatformType.Default]);

        buffsGenerator.Generate();
    }

    private void OnPlatformPassed(PlatformType type)
    {
        if (type == PlatformType.Finish)
        {
            OnFinish();
        }

        passedPlatforms.TryAdd(type, 0);

        passedPlatforms[type]++;
    }

    private void OnFinish()
    {
        GameSettings.IS_PAUSED = true;

        runnerUIManager.ShowWinScreen(passedPlatforms);
    }

    private void SetupUI(UIManager uiManager, RunnerUIManager runnerUIManager)
    {
        this.uiManager = uiManager;
        this.runnerUIManager = runnerUIManager;
        runnerUIManager.OnTapToStart += StartGame;

        player.OnSpeedChange += runnerUIManager.PlayerStatsScreen.UpdateSpeedText;
        player.OnHealthChange += runnerUIManager.PlayerStatsScreen.UpdateHealthText;

        runnerUIManager.PlayerStatsScreen.UpdateSpeedText(player.Speed);
        runnerUIManager.PlayerStatsScreen.UpdateHealthText(player.Health);

        runnerUIManager.OnContinueAfterLoose += OnContinueAfterLoose;
    }

    private void StartGame()
    {
        GameSettings.IS_PAUSED = false;
    }

    private void Update()
    {
        platformsSystem.Run();
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

    private void OnDestroy()
    {
        player.OnSpeedChange -= runnerUIManager.PlayerStatsScreen.UpdateSpeedText;
        player.OnHealthChange -= runnerUIManager.PlayerStatsScreen.UpdateHealthText;
    }
}