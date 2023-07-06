using System;
using UnityEngine;

namespace Modules.RunnerGame.Scripts.Level.Buff
{
    [Serializable]
    public class HealthBuff : Buff
    {
        [SerializeField] private int value;

        public int Value
        {
            get => value;
            set => this.value = value;
        }

        public override void CheckDuration()
        {
            base.CheckDuration();
            
        }

        public HealthBuff(int duration, int value) : base(duration)
        {
            this.value = value;
        }
    }
}