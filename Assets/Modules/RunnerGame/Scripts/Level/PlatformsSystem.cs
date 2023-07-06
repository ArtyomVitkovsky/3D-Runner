using System.Collections.Generic;
using UnityEngine.Events;

namespace Modules.RunnerGame.Scripts.Level
{
    public class PlatformsSystem
    {
        private Dictionary<PlatformType, List<Platform.Platform>> platforms;
        private Player.Player player;

        private List<int> passedPlatformIndices;

        public UnityAction<PlatformType> OnPlatformPassed;

        public PlatformsSystem(
            Dictionary<PlatformType, List<Platform.Platform>> platforms,
            Player.Player player,
            int platformsCount)
        {
            this.platforms = platforms;
            this.player = player;

            passedPlatformIndices = new List<int>(platformsCount);
        }

        public void Run()
        {
            foreach (var platformKVP in platforms)
            {
                foreach (var platform in platformKVP.Value)
                {
                    platform.DoWork();

                    if (platformKVP.Key == PlatformType.Missed ||
                        platformKVP.Key == PlatformType.DoubleMissed ||
                        platformKVP.Key == PlatformType.Saw ||
                        platformKVP.Key == PlatformType.Wall ||
                        platformKVP.Key == PlatformType.Finish)
                    {
                        if (passedPlatformIndices.Contains(platform.Index)) continue;

                        var zDistance = platform.Transform.position.z - player.transform.position.z;
                        if (zDistance <= 1f)
                        {
                            passedPlatformIndices.Add(platform.Index);
                            OnPlatformPassed?.Invoke(platformKVP.Key);
                        }
                    }
                }
            }
        }
    }
}