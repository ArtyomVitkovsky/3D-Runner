using System;

namespace Modules.RunnerGame.Scripts.Level.Buff
{
    [Serializable]
    public class InvincibleBuff : Buff
    {
        public InvincibleBuff(int duration) : base(duration)
        {
        }
        
        public override void CheckDuration()
        {
            base.CheckDuration();
        }

        
    }
}