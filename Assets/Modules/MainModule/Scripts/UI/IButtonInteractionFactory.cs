using UnityEngine;

namespace Modules.MainModule.Scripts.UI
{
    interface IButtonInteractionFactory
    {
        public ButtonInteraction Create();
        public ButtonInteraction Create(Transform parent);
        public ButtonInteraction Create(Vector3 startPosition);
        public ButtonInteraction Create(Vector3 startPosition, Transform parent);
        public ButtonInteraction Create(Vector3 startPosition, Vector3 rotation, Transform parent);
    }
}