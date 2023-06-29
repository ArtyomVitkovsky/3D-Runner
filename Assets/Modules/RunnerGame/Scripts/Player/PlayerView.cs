using System;
using Modules.RunnerGame.Scripts.Animation;
using UnityEngine;

namespace Modules.RunnerGame.Scripts.Player
{
    [Serializable]
    public class PlayerView
    {
        private AnimationController animationController;

        public PlayerView(AnimationController animationController)
        {
            this.animationController = animationController;
        }
        
        public void OnJump()
        {
            animationController.PlayAnimation(AnimationActionType.Jump);
        }

        public void OnRun()
        {
            animationController.PlayAnimation(AnimationActionType.Run);
        }
    }
}
