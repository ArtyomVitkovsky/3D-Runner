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

        private Vector3 platformPosition = new(0, 0, -4f);
        private float platformPositionStep;
        private Vector3 rotation;

        public UnityAction<float> OnPlatformGenerated;
        public UnityAction OnGenerationFinished;

        private Transform holder;

        private int platformIndex;

        public PlatformGenerator(
            PlatformConfigs platformConfigs,
            int platformsCount,
            Player.Player player,
            Transform holder)
        {
            this.platformConfigs = platformConfigs;
            this.platformsCount = platformsCount;

            this.player = player;

            rotation = Vector3.zero;

            this.holder = holder;
        }

        public Dictionary<PlatformType, List<Platform.Platform>> Platforms => platforms;

        public int PlatformsCount => platformIndex + 1;

        public void Generate()
        {
            platforms = new Dictionary<PlatformType, List<Platform.Platform>>();

            platformIndex = 0;
            for (int i = 0; i < 10; i++)
            {
                var platform = platformConfigs.GetPlatform(PlatformType.Default);
                GeneratePlatform(platform, platformIndex);
                platformIndex++;
            }

            for (int i = 0; i < platformsCount; i++)
            {
                var platform = platformConfigs.GetRandomPlatform();
                GeneratePlatform(platform, platformIndex);
                platformIndex++;
            }

            for (int i = 0; i < 5; i++)
            {
                var platform = platformConfigs.GetPlatform(PlatformType.Default);
                GeneratePlatform(platform, platformIndex);
                platformIndex++;
            }

            GenerateFinishPlatform(platformIndex);

            for (int i = 0; i < 5; i++)
            {
                var platform = platformConfigs.GetPlatform(PlatformType.Default);
                GeneratePlatform(platform, platformIndex);
                platformIndex++;
            }

            OnGenerationFinished?.Invoke();
        }

        private void GenerateFinishPlatform(int platformIndex)
        {
            var platform = platformConfigs.GetPlatform(PlatformType.Finish);
            GeneratePlatform(platform, platformIndex);
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
                holder);

            platformPositionStep = 2;

            switch (platform.Type)
            {
                case PlatformType.Default:
                {
                    platforms[platform.Type].Add(new Platform.Platform(platformInstance, index));
                    break;
                }
                case PlatformType.Missed:
                {
                    platforms[platform.Type].Add(new MissedPlatform(platformInstance, index));
                    break;
                }
                case PlatformType.DoubleMissed:
                {
                    platforms[platform.Type].Add(new DoubleMissedPlatform(platformInstance, index));
                    platformPositionStep = 3;
                    break;
                }
                case PlatformType.Saw:
                {
                    platforms[platform.Type].Add(new SawPlatform(platformInstance, index));
                    break;
                }
                case PlatformType.Wall:
                {
                    platforms[platform.Type].Add(new WallPlatform(platformInstance, index));
                    break;
                }
                case PlatformType.LeftRotator:
                {
                    platforms[platform.Type].Add(new RotatorPlatform(platformInstance, index, true, player));
                    rotation.y -= 90f;
                    break;
                }
                case PlatformType.RightRotator:
                {
                    platforms[platform.Type].Add(new RotatorPlatform(platformInstance, index, false, player));
                    rotation.y += 90f;
                    break;
                }
                case PlatformType.Finish:
                {
                    platforms[platform.Type].Add(new Platform.Platform(platformInstance, index));
                    break;
                }
            }

            platformInstance.transform.rotation = Quaternion.Euler(rotation);

            // platformPosition.z += platformPositionStep;
            platformPosition += platformInstance.transform.forward.normalized * platformPositionStep;

            platformInstance.transform.position = platformPosition;

            OnPlatformGenerated?.Invoke(index / (float) platformsCount);
        }
    }

    public class BuffsGenerator
    {
        private BuffConfigs buffConfigs;
    }
    
}