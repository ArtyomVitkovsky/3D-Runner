using System;
using UnityEngine;

namespace Modules.RunnerGame.Scripts.Level.Buff
{
    [Serializable]
    public class SpeedBuff : Buff
    {
        [SerializeField] private float value;

        public float Value
        {
            get => value;
            set => this.value = value;
        }

        public SpeedBuff(int duration, float value) : base(duration)
        {
            this.value = value;
        }
        public override void CheckDuration()
        {
            base.CheckDuration();

            if (currentDuration < 0)
            {
                value = 0;
            }
        }

        
    }
}