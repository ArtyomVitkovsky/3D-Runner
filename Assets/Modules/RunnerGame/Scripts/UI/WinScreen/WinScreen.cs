using System.Collections.Generic;
using Modules.MainModule.Scripts;
using Modules.MainModule.Scripts.UI.Interfaces;
using TMPro;
using UnityEngine;
using Zenject;

namespace Modules.RunnerGame.Scripts.UI.WinScreen
{
    public class WinScreen : MonoBehaviour, IScreen
    {
        [SerializeField] private ButtonInteraction continueButton;
        [SerializeField] private List<TextMeshProUGUI> passedPlatformsInfo;

        private ModulesSystem modulesSystem;
        
        private Dictionary<PlatformType, int> passedPlatforms;
        
        [Inject]
        private void Construct(ModulesSystem modulesSystem)
        {
            this.modulesSystem = modulesSystem;
        }

        private void Awake()
        {
            continueButton.AddListener(ContinueButtonHandler);
        }

        public void SetPassedPlatforms(Dictionary<PlatformType, int> passedPlatforms)
        {
            this.passedPlatforms = passedPlatforms;

            var platformIndex = 0;
            foreach (var passedPlatformsKVP in passedPlatforms)
            {
                passedPlatformsInfo[platformIndex].text = $"{passedPlatformsKVP.Key} {passedPlatformsKVP.Value}";
                platformIndex++;
            }
        }
        
        public void ContinueButtonHandler()
        {
            modulesSystem.ReloadCurrentModule();
        }

        public void Initialize()
        {
            continueButton.Initialize();
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}
