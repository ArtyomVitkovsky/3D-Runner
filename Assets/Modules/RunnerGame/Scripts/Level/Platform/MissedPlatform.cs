using System;
using UnityEngine;

namespace Modules.RunnerGame.Scripts.Level.Platform
{
    [Serializable]
    public class MissedPlatform : Platform
    {
        public MissedPlatform(GameObject platform) : base(platform)
        {
        }
    }
}