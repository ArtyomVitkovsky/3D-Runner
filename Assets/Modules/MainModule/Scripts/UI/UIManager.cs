using System.Collections.Generic;
using Modules.MainModule.Scripts.UI.Interfaces;
using Modules.MainModule.Scripts.UI.Screens;
using UnityEngine;

namespace Modules.MainModule.Scripts.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private Camera camera;
        [SerializeField] private LoadingScreen loadingScreen;

        private List<IScreen> screens;
        
        public Camera Camera => camera;

        public Canvas Canvas => canvas;

        private void Awake()
        {
            screens ??= new List<IScreen>();
            screens.Capacity = 4;
            
            screens.Add(loadingScreen);
        }

        public void AddScreen(IScreen screen)
        {
            screens ??= new List<IScreen>();
            
            if(screens.Contains(screen)) return;
            
            screens.Add(screen);
        }
        
        public void RemoveScreen(IScreen screen)
        {
            screens.Remove(screen);
        }

        public void SetScreenActive<T>(bool isActive, bool asOverlay)
        {
            foreach (var screen in screens)
            {
                var screenTypeMatched = screen.GetType() == typeof(T);

                if (asOverlay && !screenTypeMatched)
                {
                    continue;
                }
                screen.SetActive(isActive && screenTypeMatched);
            }
        }
    }
}