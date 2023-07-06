using System;
using Modules.RunnerGame.Scripts.Level.Buff;
using UnityEngine;

namespace Modules.RunnerGame.Scripts.Setup
{
    [Serializable]
    public class InvincibleBuffConfig : BuffConfig
    {
        [SerializeField] InvincibleBuff invincibleBuff;

        public InvincibleBuff InvincibleBuff => invincibleBuff;
    }
}