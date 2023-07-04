using System;
using UnityEngine;

namespace Modules.RunnerGame.Scripts.Level.Platform
{
    [Serializable]
    public class DoubleMissedPlatform : Platform
    {
        public DoubleMissedPlatform(GameObject platform, int index) : base(platform, index)
        {
        }
    }
}