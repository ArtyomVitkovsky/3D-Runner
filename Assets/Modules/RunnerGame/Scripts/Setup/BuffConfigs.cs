using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Modules.RunnerGame.Scripts.Setup
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Buffs", fileName = "BuffConfigs")]
    public class BuffConfigs : ScriptableObject
    {
        [SerializeField] private HealthBuffConfig healthBuff;
        [SerializeField] private SpeedBuffConfig speedBuff;
        [SerializeField] private InvincibleBuffConfig invincibleBuff;

        private List<BuffConfig> sortedConfigs;

        public void Initialize()
        {
            Sort();
        }
        
        public BuffConfig GetRandomBuff()
        {
            BuffConfig result = null;
            double chance = Random.Range(0, 101);

            double cumulative = 0.0;

            foreach (var config in sortedConfigs)
            {
                if (CheckChance(config, chance, cumulative, out cumulative))
                {
                    result = config;
                    break;
                }
            }
            
            
            return result;
        }

        private bool CheckChance(BuffConfig buffConfig, double chance, double currentCumulative, out double cumulative)
        {
            cumulative = currentCumulative;
            cumulative += buffConfig.Chance;
            return chance < cumulative;
        }

        private void Sort()
        {
            sortedConfigs = new List<BuffConfig>
            {
                healthBuff,
                speedBuff,
                invincibleBuff
            };


            sortedConfigs = sortedConfigs.OrderBy(b => b.Chance).ToList();
        }
    }
}