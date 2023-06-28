using System;
using System.Collections;
using System.Collections.Generic;
using Modules.MainModule.Scripts.UI;
using Modules.MainModule.Scripts.UI.Screens;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Zenject;

namespace Modules.MainModule.Scripts
{
    public class SceneLoader : MonoBehaviour
    {
        private List<Scene> loadedScenes;
        
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
            loadedScenes ??= new List<Scene>();
        }

        public void LoadScene(string sceneName, LoadSceneMode loadSceneMode)
        {
            if(loadSceneMode == LoadSceneMode.Single) loadedScenes.Clear();

            StartCoroutine(LoadSceneCoroutine(sceneName, loadSceneMode, false));
        }

        private IEnumerator LoadSceneCoroutine(string sceneName, LoadSceneMode loadSceneMode, bool canBeLoadedTwice)
        {
            if (SceneManager.GetSceneByName(sceneName).isLoaded && !canBeLoadedTwice) yield break;

            uiManager.SetScreenActive<LoadingScreen>(true, false);
            
            yield return SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
            
            var loadedScene = SceneManager.GetSceneByName(sceneName);
            
            loadedScenes ??= new List<Scene>();
            loadedScenes.Add(loadedScene);

            SceneManager.SetActiveScene(loadedScene);
            
            OnSceneLoaded?.Invoke(sceneName);
        }

        public void UnloadScene(string sceneName)
        {
            StartCoroutine(UnloadSceneCoroutine(sceneName));
        }

        private IEnumerator UnloadSceneCoroutine(string sceneName)
        {
            yield return SceneManager.UnloadSceneAsync(sceneName);
            
            OnSceneUnloaded?.Invoke(sceneName);
        }

        public void ReloadScene(string sceneName)
        {
            StartCoroutine(ReloadSceneCoroutine(sceneName));
        }

        private IEnumerator ReloadSceneCoroutine(string sceneName)
        {
            var lastActiveScene = SceneManager.GetActiveScene();
            yield return StartCoroutine(LoadSceneCoroutine(sceneName, LoadSceneMode.Additive, true));
            yield return SceneManager.UnloadSceneAsync(lastActiveScene);
        }
    }
}