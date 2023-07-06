using System;
using UnityEngine;
using UnityEngine.Events;

namespace Modules.RunnerGame.Scripts.Level.Buff
{
    [Serializable]
    public class Buff
    {
        [SerializeField] protected int duration;
        [SerializeField] protected float currentDuration;

        private GameObject gameObject;

        protected bool isActive;

        public int Duration => duration;

        public bool IsActive => isActive;

        public UnityAction<Buff> OnBuffEnded;

        public Buff(int duration)
        {
            this.duration = duration;
            currentDuration = duration;
        }

        public virtual void CheckDuration()
        {
            if (duration == -1)
            {
                isActive = true;
            }
            else if (currentDuration >= 0)
            {
                currentDuration -= Time.deltaTime;
                isActive = true;
            }
            else if (IsActive)
            {
                isActive = false;

                OnBuffEnded?.Invoke(this);
            }
        }
    }
}