using System;
using Modules.RunnerGame.Scripts.Level.Buff;
using UnityEngine;

namespace Modules.RunnerGame.Scripts.Setup
{
    [Serializable]
    public class HealthBuffConfig : BuffConfig
    {
        [SerializeField] HealthBuff healthBuff;

        public HealthBuff HealthBuff => healthBuff;
    }
}