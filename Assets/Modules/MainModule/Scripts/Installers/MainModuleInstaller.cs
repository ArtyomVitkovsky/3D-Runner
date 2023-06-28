using Modules.MainModule.Scripts.InputServices;
using Modules.MainModule.Scripts.Sound;
using Modules.MainModule.Scripts.UI;
using UnityEngine;
using Zenject;

namespace Modules.MainModule.Scripts.Installers
{
    public class MainModuleInstaller : MonoInstaller
    {
        [Header("SAVES")]
        [SerializeField] private PlayerData playerData;
        
        [Header("INPUT SERVICES")]
        [SerializeField] private MobileInputService mobileInputService;
        [SerializeField] private PcInputService pcInputService;
        
        [Header("SCENE MANAGEMENT")]
        [SerializeField] private SceneLoader sceneLoader;
        
        [Header("MODULES SYSTEM")]
        [SerializeField] private ModulesSystem modulesSystemPrefab;

        [Header("OUTPUT")] 
        [SerializeField] private SoundControl soundControl;
        
        [Header("USER INTERFACE")]
        [SerializeField] private UIManager uiManager;
        
        public override void InstallBindings()
        {
            BindPlayerData();

            BindInputService();
            
            BindSoundControl();

            BindUIManager();

            BindSceneLoader();
            
            BindModulesSystem();
        }

        private void BindSoundControl()
        {
            var soundControlInstance = Container
                .InstantiatePrefabForComponent<SoundControl>(soundControl);

            Container.Bind<SoundControl>().FromInstance(soundControlInstance).AsSingle();
        }

        private void BindPlayerData()
        {
            var playerDataInstance = Container
                .InstantiatePrefabForComponent<PlayerData>(playerData);
            
            playerDataInstance.Initialize();

            Container.Bind<PlayerData>().FromInstance(playerDataInstance).AsSingle();
        }


        private void BindModulesSystem()
        {
            var modulesSystemInstance = Container
                .InstantiatePrefabForComponent<ModulesSystem>(modulesSystemPrefab);

            Container.Bind<ModulesSystem>().FromInstance(modulesSystemInstance).AsSingle();
        }

        private void BindInputService()
        {
            InputService inputServiceInstance;

#if UNITY_ANDROID || UNITY_IOS

            inputServiceInstance = Container
            .InstantiatePrefabForComponent<MobileInputService>(mobileInputService);

#endif
            
#if UNITY_EDITOR
            
            inputServiceInstance = Container
                .InstantiatePrefabForComponent<PcInputService>(pcInputService);
#endif

            Container
                .Bind<InputService>()
                .FromInstance(inputServiceInstance)
                .AsSingle();
        }

        private void BindSceneLoader()
        {
            var sceneLoaderInstance = Container
                .InstantiatePrefabForComponent<SceneLoader>(sceneLoader);

            Container
                .Bind<SceneLoader>()
                .FromInstance(sceneLoaderInstance)
                .AsSingle();
        }
        
        private void BindUIManager()
        {
            var uiManagerInstance = Container
                .InstantiatePrefabForComponent<UIManager>(uiManager);

            Container
                .Bind<UIManager>()
                .FromInstance(uiManagerInstance)
                .AsSingle();
        }
    }
}