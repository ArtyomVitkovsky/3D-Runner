using System;
using Modules.RunnerGame.Scripts.Level.Buff;
using UnityEngine;

namespace Modules.RunnerGame.Scripts.Setup
{
    [Serializable]
    public class SpeedBuffConfig : BuffConfig
    {
        [SerializeField] SpeedBuff speedBuff;

        public SpeedBuff SpeedBuff => speedBuff;
    }
}