using System;
using UnityEngine;

namespace Modules.RunnerGame.Scripts.Level.Platform
{
    [Serializable]
    public class Platform
    {
        [SerializeField] protected GameObject platform;

        public Transform Transform => platform.transform;
        public Platform(GameObject platform)
        {
            this.platform = platform;
        }

        public virtual void DoWork(){}
    }
}