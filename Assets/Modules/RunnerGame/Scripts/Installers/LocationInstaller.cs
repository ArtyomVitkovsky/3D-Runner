using Modules.RunnerGame.Scripts.UI;
using UnityEngine;
using Zenject;

namespace Modules.RunnerGame.Scripts.Installers
{
    public class LocationInstaller : MonoInstaller
    {
        [SerializeField] private Player.Player playerPrefab;
        [SerializeField] private PlayerCamera.PlayerCamera playerCameraPrefab;
        [SerializeField] private RunnerUIManager runnerUIManager;

        public override void InstallBindings()
        {
            BindPlayer();
            BindPlayerCamera();
            BindRunnerUI();
        }

        private void BindPlayer()
        {
            var playerInstance = Container
                .InstantiatePrefabForComponent<Player.Player>(playerPrefab);

            Container.Bind<Player.Player>().FromInstance(playerInstance).AsSingle();
        }
    
        private void BindPlayerCamera()
        {
            var playerCameraInstance = Container
                .InstantiatePrefabForComponent<PlayerCamera.PlayerCamera>(playerCameraPrefab);

            Container.Bind<PlayerCamera.PlayerCamera>().FromInstance(playerCameraInstance).AsSingle();
        }
        
        private void BindRunnerUI()
        {
            Container.Bind<RunnerUIManager>().FromInstance(runnerUIManager).AsSingle();
        }
    }
}
