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
            var platformIndex = 0;
            foreach (var passedPlatformsKVP in passedPlatforms)
            {
                passedPlatformsInfo[platformIndex].gameObject.SetActive(true);
                passedPlatformsInfo[platformIndex].text = $"{passedPlatformsKVP.Key} {passedPlatformsKVP.Value}";
                platformIndex++;
            }

            for (int i = platformIndex; i < passedPlatformsInfo.Count; i++)
            {
                passedPlatformsInfo[platformIndex].gameObject.SetActive(false);
            }
        }

        private void ContinueButtonHandler()
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
