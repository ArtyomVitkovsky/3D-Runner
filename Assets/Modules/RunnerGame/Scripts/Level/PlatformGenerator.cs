using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using Modules.RunnerGame.Scripts.Level.Platform;
using UnityEngine;
using UnityEngine.Events;

namespace Modules.RunnerGame.Scripts.Level
{
    [Serializable]
    public class PlatformGenerator
    {
        private PlatformConfigs platformConfigs;
        private int platformsCount;

        private Player.Player player;

        private Dictionary<PlatformType, List<Platform.Platform>> platforms;

        private Vector3 platformPosition;
        private Vector3 rotation;

        public UnityAction<float> OnPlatformGenerated;
        public UnityAction OnGenerationFinished;

        private Transform holder;

        public UnityAction<PlatformType> OnPlatformPassed;

        public PlatformGenerator(PlatformConfigs platformConfigs, int platformsCount, Player.Player player,
            Transform holder)
        {
            this.platformConfigs = platformConfigs;
            this.platformsCount = platformsCount;

            this.player = player;

            rotation = Vector3.zero;

            this.holder = holder;
        }

        public void Generate()
        {
            platforms = new Dictionary<PlatformType, List<Platform.Platform>>();
            platformPosition = Vector3.zero;

            for (int i = 0; i < 10; i++)
            {
                var platform = platformConfigs.GetPlatform(PlatformType.Default);
                GeneratePlatform(platform, i);
            }

            for (int i = 0; i < platformsCount; i++)
            {
                var platform = platformConfigs.GetRandomPlatform();
                GeneratePlatform(platform, i);
            }
            
            for (int i = 0; i < 10; i++)
            {
                var platform = platformConfigs.GetPlatform(PlatformType.Default);
                GeneratePlatform(platform, i);
            }
            
            OnGenerationFinished?.Invoke();
        }

        private void GeneratePlatform(PlatformConfig platform, int index)
        {
            if (platform.Prefab == null) return;

            if (!platforms.ContainsKey(platform.Type))
            {
                platforms.Add(platform.Type, new List<Platform.Platform>());
            }

            var platformInstance = GameObject.Instantiate(
                platform.Prefab,
                platformPosition,
                Quaternion.Euler(rotation),
                holder);

            switch (platform.Type)
            {
                case PlatformType.Default:
                {
                    platforms[platform.Type].Add(new Platform.Platform(platformInstance));
                    break;
                }
                case PlatformType.Missed:
                {
                    platforms[platform.Type].Add(new MissedPlatform(platformInstance));
                    break;
                }
                case PlatformType.DoubleMissed:
                {
                    platforms[platform.Type].Add(new DoubleMissedPlatform(platformInstance));
                    break;
                }
                case PlatformType.Saw:
                {
                    platforms[platform.Type].Add(new SawPlatform(platformInstance));
                    break;
                }
                case PlatformType.Wall:
                {
                    platforms[platform.Type].Add(new WallPlatform(platformInstance));
                    break;
                }
                case PlatformType.LeftRotator:
                {
                    platforms[platform.Type].Add(new RotatorPlatform(platformInstance, true, player));
                    rotation.y -= 90f;

                    break;
                }
                case PlatformType.RightRotator:
                {
                    platforms[platform.Type].Add(new RotatorPlatform(platformInstance, false, player));
                    rotation.y += 90f;

                    break;
                }
            }

            platformPosition.z += 2f;

            OnPlatformGenerated?.Invoke(index / (float) platformsCount);
        }

        public void Update()
        {
            foreach (var platformKVP in platforms)
            {
                foreach (var platform in platformKVP.Value)
                {
                    platform.DoWork();

                    if (platformKVP.Key == PlatformType.Missed ||
                        platformKVP.Key == PlatformType.DoubleMissed ||
                        platformKVP.Key == PlatformType.Saw ||
                        platformKVP.Key == PlatformType.Wall)
                    {
                        if (Vector3.Distance(platform.Transform.position, player.transform.position) <= 2f)
                        {
                            OnPlatformPassed?.Invoke(platformKVP.Key);
                        }
                    }
                }
            }
        }
    }
}