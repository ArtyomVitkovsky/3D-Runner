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

        private ModuleSo currentModule;

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
            currentModule = startModule;
            sceneLoader.LoadScene(startModule.SceneName, LoadSceneMode.Single);
        }
        
        public void LoadModule(Module module)
        {
            var moduleSo = modules.FirstOrDefault(m => m.Module == module);
            if(moduleSo == null) return;
            
            currentModule = moduleSo;
            
            sceneLoader.LoadScene(moduleSo.SceneName, LoadSceneMode.Additive);
        }
        
        public void ReloadModule(Module module)
        {
            var moduleSo = modules.FirstOrDefault(m => m.Module == module);
            if(moduleSo == null) return;
            
            sceneLoader.ReloadScene(moduleSo.SceneName);
        }
        
        public void ReloadCurrentModule()
        {
            sceneLoader.ReloadScene(currentModule.SceneName);
        }
    }
}
