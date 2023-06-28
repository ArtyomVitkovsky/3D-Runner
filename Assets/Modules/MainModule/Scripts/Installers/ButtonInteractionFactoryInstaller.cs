using Modules.MainModule.Scripts.UI;
using Modules.MainModule.Scripts.UI.Button;
using UnityEngine;
using Zenject;

namespace Modules.MainModule.Scripts.Installers
{
    public class ButtonInteractionFactoryInstaller : MonoInstaller
    {
        [SerializeField] private ButtonInteractionFactory ButtonInteractionFactory;

        public override void InstallBindings()
        {
            Container.Bind<IButtonInteractionFactory>().FromInstance(ButtonInteractionFactory).AsSingle();
        }
    }
}