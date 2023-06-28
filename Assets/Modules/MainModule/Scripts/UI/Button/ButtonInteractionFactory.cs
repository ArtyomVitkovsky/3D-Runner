using UnityEngine;
using Zenject;

namespace Modules.MainModule.Scripts.UI.Button
{
    public class ButtonInteractionFactory : MonoBehaviour, IButtonInteractionFactory
    {
        [SerializeField] private ButtonInteraction ButtonInteractionPrefab;
        [Inject] private IInstantiator instantiator;

        public ButtonInteraction Create()
        {
            var ButtonInteraction =
                instantiator.InstantiatePrefabForComponent<ButtonInteraction>(ButtonInteractionPrefab);
            return ButtonInteraction;
        }

        public ButtonInteraction Create(Transform parent)
        {
            var ButtonInteraction =
                instantiator.InstantiatePrefabForComponent<ButtonInteraction>(ButtonInteractionPrefab, parent);
            return ButtonInteraction;
        }

        public ButtonInteraction Create(Vector3 startPosition)
        {
            var ButtonInteraction = instantiator
                .InstantiatePrefabForComponent<ButtonInteraction>(
                    ButtonInteractionPrefab,
                    startPosition,
                    Quaternion.identity,
                    null);

            return ButtonInteraction;
        }

        public ButtonInteraction Create(Vector3 startPosition, Transform parent)
        {
            var ButtonInteraction = instantiator
                .InstantiatePrefabForComponent<ButtonInteraction>(
                    ButtonInteractionPrefab,
                    startPosition,
                    Quaternion.identity,
                    parent);

            return ButtonInteraction;
        }

        public ButtonInteraction Create(Vector3 startPosition, Vector3 rotation, Transform parent)
        {
            var ButtonInteraction = instantiator
                .InstantiatePrefabForComponent<ButtonInteraction>(
                    ButtonInteractionPrefab,
                    startPosition,
                    Quaternion.Euler(rotation),
                    parent);

            return ButtonInteraction;
        }
    }
}