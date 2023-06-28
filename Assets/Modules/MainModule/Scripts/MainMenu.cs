using Modules.MainModule.Scripts.UI;
using UnityEngine;
using Zenject;

namespace Modules.MainModule.Scripts
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private ModulesMenuScreen modulesMenuScreen;
        
        private UIManager uiManager;
        private ModulesSystem modulesSystem;
        [Inject]
        private void Construct(UIManager uiManager, ModulesSystem modulesSystem)
        {
            this.uiManager = uiManager;
            this.modulesSystem = modulesSystem;

            canvas.renderMode = this.uiManager.Canvas.renderMode;
            canvas.worldCamera = this.uiManager.Camera;
            canvas.planeDistance = this.uiManager.Canvas.planeDistance;
        }
        
        void Start()
        {
            uiManager.AddScreen(modulesMenuScreen);
            uiManager.SetScreenActive<ModulesMenuScreen>(true, false);
            
            modulesMenuScreen.Initialize(modulesSystem.Modules);

            foreach (var button in modulesMenuScreen.ButtonsModulesMap.Keys)
            {
                button.onButtonClick += OnButtonClick;
            }

        }
        
        
        private void OnButtonClick(ButtonInteraction buttonInteraction)
        {
            modulesSystem.LoadModule(modulesMenuScreen.ButtonsModulesMap[buttonInteraction]);
        }

        private void OnDestroy()
        {
            if(modulesMenuScreen == null) return;
            if(modulesMenuScreen.ButtonsModulesMap == null) return;
            
            foreach (var button in modulesMenuScreen.ButtonsModulesMap.Keys)
            {
                button.onButtonClick += OnButtonClick;
            }
        }
    }
}
