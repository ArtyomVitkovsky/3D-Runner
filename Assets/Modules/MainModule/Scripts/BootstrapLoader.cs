using System.Collections.Generic;
using Modules.MainModule.Scripts.UI;
using Modules.MainModule.Scripts.UI.Screens;
using UnityEngine;
using Zenject;

namespace Modules.MainModule.Scripts
{
    public class BootstrapLoader : MonoBehaviour
    {
        private ModulesSystem modulesSystem;
        private UIManager uiManager;
        [Inject]
        private void Construct(ModulesSystem modulesSystem, UIManager uiManager)
        {
            this.modulesSystem = modulesSystem;
            this.uiManager = uiManager;
            
            this.uiManager.SetScreenActive<LoadingScreen>(true, false);
        }
    }
}
