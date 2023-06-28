using Modules.MainModule.Scripts.UI.Interfaces;
using UnityEngine;

namespace Modules.MainModule.Scripts.UI.Screens
{
    public class LoadingScreen : MonoBehaviour, IScreen
    {
        public void Initialize()
        {
            
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}