using UnityEngine;
using Zenject;

namespace Modules.MainModule.Scripts
{
    public class BootstrapLoaderInstaller : MonoInstaller
    {
        [SerializeField] private BootstrapLoader bootstrapLoaderPrefab;
        public override void InstallBindings()
        {
            var bootstrapLoaderInstance = Container
                .InstantiatePrefabForComponent<BootstrapLoader>(bootstrapLoaderPrefab);
    
            Container.Bind<BootstrapLoader>().FromInstance(bootstrapLoaderInstance).AsSingle();
        }
    }
}