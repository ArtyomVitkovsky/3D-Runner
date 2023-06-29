using UnityEngine;

namespace Modules.RunnerGame.Scripts.PlayerCamera
{
    public abstract class Follower : MonoBehaviour
    {
        [SerializeField] protected Transform followTarget;
        [SerializeField] protected float followSpeed;
        [SerializeField] protected Vector3 offset;
        protected bool isActive;

        protected void Follow(float deltaTime)
        {
            var targetPosition = Vector3.Lerp(transform.position, followTarget.position, deltaTime * followSpeed);
            transform.position = targetPosition;

            transform.rotation = followTarget.rotation;
        }
        
        public void SetFollowTarget(Transform followTarget)
        {
            this.followTarget = followTarget;
            isActive = this.followTarget != null;

        }
    }
}