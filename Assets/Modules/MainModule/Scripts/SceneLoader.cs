using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Modules.MainModule.Scripts.UI;
using Modules.MainModule.Scripts.UI.Screens;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Zenject;

namespace Modules.MainModule.Scripts
{
    public class SceneLoader : MonoBehaviour
    {
        private List<SceneInstance> loadedScenes;

        private SceneInstance loadedSceneInstance;
        
        private UIManager uiManager;
        [Inject]
        private void Construct(UIManager uiManager)
        {
            this.uiManager = uiManager;
        }

        public UnityAction<string> OnSceneLoaded;
        public UnityAction<string> OnSceneUnloaded;

        private void Awake()
        {
            loadedScenes ??= new List<SceneInstance>();
        }

        public void LoadScene(string sceneName, LoadSceneMode loadSceneMode)
        {
            if (loadSceneMode == LoadSceneMode.Single) loadedScenes.Clear();

            LoadSceneAsync(sceneName, loadSceneMode, false);
        }

        private async UniTask LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode, bool canBeLoadedTwice)
        {
            if (SceneManager.GetSceneByName(sceneName).isLoaded && !canBeLoadedTwice) return;

            uiManager.SetScreenActive<LoadingScreen>(true, false);

            var asyncHandle = Addressables.LoadSceneAsync(sceneName, loadSceneMode);
            await UniTask.WaitWhile(() => !asyncHandle.IsDone);

            loadedSceneInstance = asyncHandle.Result;
            
            var loadedScene = SceneManager.GetSceneByName(sceneName);

            loadedScenes ??= new List<SceneInstance>();
            loadedScenes.Add(loadedSceneInstance);

            SceneManager.SetActiveScene(loadedScene);

            OnSceneLoaded?.Invoke(sceneName);
        }

        public void UnloadSceneAsync(string sceneName)
        {
            UnloadSceneCoroutine(sceneName);
        }

        private async UniTask UnloadSceneCoroutine(string sceneName)
        {
            var sceneInstance = loadedScenes.Find(s => s.Scene.name == sceneName);
            
            await UniTask.WaitWhile(() => !Addressables.UnloadSceneAsync(sceneInstance).IsDone);

            OnSceneUnloaded?.Invoke(sceneName);
        }

        public void ReloadScene(string sceneName)
        {
            ReloadSceneCoroutine(sceneName);
        }

        private async UniTask ReloadSceneCoroutine(string sceneName)
        {
            await UniTask.WaitWhile(() => !Addressables.UnloadSceneAsync(loadedSceneInstance).IsDone);
            LoadSceneAsync(sceneName, LoadSceneMode.Additive, true);
        }
    }
}