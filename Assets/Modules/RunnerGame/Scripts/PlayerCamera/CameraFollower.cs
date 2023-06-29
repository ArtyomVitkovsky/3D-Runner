using UnityEngine;

namespace Modules.RunnerGame.Scripts.PlayerCamera
{
    public class CameraFollower : Follower
    {
        [SerializeField] protected Transform cameraTransform;

        private void Start()
        {
            cameraTransform.localPosition = offset;
        }

        protected void FixedUpdate()
        {
            Follow(Time.fixedDeltaTime);
        }
    }
}
