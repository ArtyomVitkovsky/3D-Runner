using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Modules.RunnerGame.Scripts.Setup
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Platforms", fileName = "PlatformConfigs")]
    public class PlatformConfigs : ScriptableObject
    {
        [SerializeField] private PlatformConfig[] platformConfigs;

        public PlatformConfig GetRandomPlatform()
        {
            PlatformConfig result = new PlatformConfig();
            double chance = Random.Range(0, 101);

            double cumulative = 0.0;
            for (int i = 0; i < platformConfigs.Length; i++)
            {
                cumulative += platformConfigs[i].SpawnChance;
                if (chance < cumulative)
                {
                    result = platformConfigs[i];
                    break;
                }
            }
        
            return result;
        }

        public PlatformConfig GetPlatform(PlatformType platformType)
        {
            return platformConfigs.FirstOrDefault(c => c.Type == platformType);
        }
    }
}