using System;
using UnityEngine;

namespace Modules.RunnerGame.Scripts.Level.Platform
{
    [Serializable]
    public class Platform
    {
        [SerializeField] protected GameObject platform;

        protected int index;

        public Transform Transform => platform.transform;

        public int Index => index;

        public Platform(GameObject platform, int index)
        {
            this.platform = platform;
            this.index = index;

            var damager = platform.GetComponentInChildren<Damager>();
            if (damager != null)
            {
                damager.SetPlatform(this);
            }
        }

        public virtual void DoWork(){}
    }
}