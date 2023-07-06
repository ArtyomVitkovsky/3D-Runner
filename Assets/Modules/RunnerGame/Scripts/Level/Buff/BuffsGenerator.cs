using System.Collections.Generic;
using Modules.RunnerGame.Scripts.Setup;
using UnityEngine;

namespace Modules.RunnerGame.Scripts.Level.Buff
{
    public class BuffsGenerator
    {
        private BuffConfigs buffConfigs;

        private List<BuffObject> buffObjects;
        
        private List<Platform.Platform> defaultPlatforms;

        public BuffsGenerator(BuffConfigs buffConfigs, List<Platform.Platform> defaultPlatforms)
        {
            this.buffConfigs = buffConfigs;
            this.defaultPlatforms = defaultPlatforms;

            buffObjects = new List<BuffObject>();
        }

        public List<BuffObject> BuffObjects => buffObjects;

        public void Generate()
        {
            buffConfigs.Initialize();
            
            var index = 0;
            foreach (var platform in defaultPlatforms)
            {
                var buffConfig = buffConfigs.GetRandomBuff();
                
                if(buffConfig == null) continue;
                
                var buffInstance = GameObject.Instantiate(
                    buffConfig.Prefab,
                    platform.Transform);

                Buff buff = null;
                if (buffConfig is HealthBuffConfig healthBuffCfg)
                {
                    buff = new HealthBuff(healthBuffCfg.HealthBuff.Duration, healthBuffCfg.HealthBuff.Value);
                }
                else if (buffConfig is SpeedBuffConfig speedBuffCfg)
                {
                    buff = new SpeedBuff(speedBuffCfg.SpeedBuff.Duration, speedBuffCfg.SpeedBuff.Value);
                }
                else if (buffConfig is InvincibleBuffConfig invincibleBuffCfg)
                {
                    buff = new InvincibleBuff(invincibleBuffCfg.InvincibleBuff.Duration);
                }

                var buffObject = buffInstance.AddComponent<BuffObject>();
                buffObject.Construct(index, buffInstance, buff);
                
                buffObjects.Add(buffObject);
            }
            
        }
    }
}