using System;
using UnityEngine;

namespace Modules.RunnerGame.Scripts.Level.Platform
{
    [Serializable]
    public class RotatorPlatform : Platform
    {
        private bool isLeftRotator;
        private Player.Player player;

        public RotatorPlatform(GameObject platform, bool isLeftRotator, Player.Player player) : base(platform)
        {
            this.isLeftRotator = isLeftRotator;
            this.player = player;
        }

        // public override void DoWork()
        // {
        //     var lookDirection = isLeftRotator ? -player.transform.right : player.transform.right;
        //     player.transform.LookAt(lookDirection);
        // }
    }
}