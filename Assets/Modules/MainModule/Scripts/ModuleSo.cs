using Modules.MainModule.Scripts.Enums;
using UnityEngine;

namespace Modules.MainModule.Scripts
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Modules", fileName = "Module_0")]
    public class ModuleSo : ScriptableObject
    {
        [SerializeField] private Module module;
        [SerializeField] private string moduleName;
        [SerializeField] private string sceneName;

        public Module Module => module;

        public string SceneName => sceneName;

        public string ModuleName => moduleName;
    }
}