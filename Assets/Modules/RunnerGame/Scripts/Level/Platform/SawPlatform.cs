using System;
using UnityEngine;

namespace Modules.RunnerGame.Scripts.Level.Platform
{
    [Serializable]
    public class SawPlatform : Platform
    {
        [SerializeField] private GameObject saw;
        private Transform sawTransform;

        private float[] sawClamp = {-1.5f, 1.5f};

        private float targetPosX;

        private float factor;
        private int posIndex;

        public SawPlatform(GameObject platform, int index) : base(platform, index)
        {
            this.platform = platform;
            saw = platform.transform.GetChild(1).gameObject;
            sawTransform = saw.transform;

            posIndex = 0;
            targetPosX = sawClamp[posIndex];
        }
    
        public override void DoWork()
        { 
            sawTransform = saw.transform;
            factor += Time.deltaTime;
            var position = 
                Vector3.Lerp(
                    sawTransform.localPosition, 
                    new Vector3(targetPosX, 0, 0), 
                    factor);

            sawTransform.localPosition = position;

            if (factor >= 1)
            {
                posIndex++;
                if (posIndex >= sawClamp.Length)
                {
                    posIndex = 0;
                }
                
                targetPosX = sawClamp[posIndex];
                factor = 0;
            }
        }
    }
}