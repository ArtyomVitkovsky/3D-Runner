using System;
using UnityEngine;

namespace Modules.MainModule.Scripts.UI.Button
{
    [Serializable]
    public class ButtonView
    {
        [SerializeField] private Sprite sprite;
        [SerializeField] private ButtonViewType viewType;
    
        public Sprite Sprite => sprite;

        public ButtonViewType ViewType => viewType;
    }
}