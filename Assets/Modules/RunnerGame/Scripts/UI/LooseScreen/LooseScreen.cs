using System.Collections.Generic;
using Modules.MainModule.Scripts;
using Modules.MainModule.Scripts.UI.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Modules.RunnerGame.Scripts.UI.LooseScreen
{
    public class LooseScreen : MonoBehaviour, IScreen
    {
        [SerializeField] private ButtonInteraction continueButton;
        [SerializeField] private ButtonInteraction restartButton;
        [SerializeField] private List<TextMeshProUGUI> passedPlatformsInfo;

        private ModulesSystem modulesSystem;
        
        public UnityAction OnContinue;

        [Inject]
        private void Construct(ModulesSystem modulesSystem)
        {
            this.modulesSystem = modulesSystem;
        }

        private void Awake()
        {
            continueButton.AddListener(ContinueButtonHandler);
            restartButton.AddListener(RestartButtonHandler);
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
        
        public void ContinueButtonHandler()
        {
            OnContinue?.Invoke();
        }
        
        public void RestartButtonHandler()
        {
            modulesSystem.ReloadCurrentModule();
        }

        public void Initialize()
        {
            continueButton.Initialize();
            restartButton.Initialize();
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}