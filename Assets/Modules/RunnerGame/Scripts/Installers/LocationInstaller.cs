using UnityEngine;
using Zenject;

namespace Modules.RunnerGame.Scripts.Installers
{
    public class LocationInstaller : MonoInstaller
    {
        [SerializeField] private Player.Player playerPrefab;
        [SerializeField] private PlayerCamera.PlayerCamera playerCameraPrefab;

        public override void InstallBindings()
        {
            BindPlayer();
            BindPlayerCamera();
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
    }
}
