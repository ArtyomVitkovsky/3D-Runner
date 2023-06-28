using System;
using System.Linq;
using Modules.MainModule.Scripts.Enums;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;


namespace Modules.MainModule.Scripts
{
    public class ModulesSystem : MonoBehaviour
    {
        [SerializeField] private ModuleSo startModule;
        
        [SerializeField] private ModuleSo[] modules;

        private SceneLoader sceneLoader;
        
        public ModuleSo[] Modules => modules;

        [Inject]
        private void Construct(SceneLoader sceneLoader)
        {
            this.sceneLoader = sceneLoader;
        }

        private void Start()
        {
            LoadStartModule();
        }

        public void LoadStartModule()
        {
            sceneLoader.LoadScene(startModule.SceneName, LoadSceneMode.Single);
        }
        
        public void LoadModule(Module module)
        {
            var moduleSo = modules.FirstOrDefault(m => m.Module == module);
            if(moduleSo == null) return;
            
            sceneLoader.LoadScene(moduleSo.SceneName, LoadSceneMode.Additive);
        }
    }
}
