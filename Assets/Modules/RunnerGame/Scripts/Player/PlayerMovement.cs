using System;
using JetBrains.Annotations;
using Modules.MainModule.Scripts.InputServices;
using UnityEngine;
using UnityEngine.Events;

namespace Modules.RunnerGame.Scripts.Player
{
    [Serializable]
    public class PlayerMovement
    {
        private Rigidbody rigidbody;
        private Transform transform;
        private InputService inputService;

        private float speed;
        private float jumpForce;
        
        private Vector3 moveDirection;

        private bool isGrounded;
        private bool isJumped;
        [SerializeField] private int jumpsCount;

        public UnityAction OnGrounded;
        public UnityAction OnJump;

        private LayerMask groundMask;
        [SerializeField] private float rayLength = 0.1f;

        public float Speed => speed;

        public PlayerMovement(InputService inputService, Rigidbody rigidbody, Transform transform, 
            float speed, float jumpForce, LayerMask groundMask)
        {
            this.inputService = inputService;
            this.rigidbody = rigidbody;
            this.transform = transform;

            this.speed = speed;
            this.jumpForce = jumpForce;

            this.groundMask = groundMask;
            
            this.inputService.OnTap += Jump;
        
            SetMoveDirectionZ(1f);
        }

        private void Jump()
        {
            if (GameSettings.IS_PAUSED) return;
            
            if (isGrounded || jumpsCount < 2)
            {
                jumpsCount++;

                OnJump?.Invoke();

                rigidbody.AddForce(0,jumpForce, 0, ForceMode.Impulse);

                isJumped = true;
            }
        }

        private void SetMoveDirectionX(float x)
        {
            moveDirection.x = x;
        }

        private void SetMoveDirectionZ(float z)
        {
            moveDirection.z = z;
        }

        public void Move()
        {
            if (!isGrounded)
            {
                Gravity();
            }
            else if (isJumped)
            {
                jumpsCount = 0;
                isJumped = false;
            }
            
            var targetVelocity = rigidbody.velocity;
            targetVelocity.z = moveDirection.z * speed;
            rigidbody.velocity = targetVelocity;
        }

        public void Stop()
        {
            rigidbody.velocity = Vector3.zero;
        }

        public void IsGrounded()
        {
            var startPos = transform.position;
            var direction = startPos - transform.up;

            Physics.Raycast(startPos, direction, out var hitInfo, rayLength, groundMask);

            Debug.DrawLine(startPos, direction);
            
            isGrounded = hitInfo.transform != null;
        }

        private void Gravity()
        {
            rigidbody.AddForce(new Vector3(0, -9.81f, 0));
        }

        public void OnDestroy()
        {
            inputService.OnTap -= Jump;
        }

        public void SetSpeed(float speed)
        {
            this.speed = speed;
        }
    }
}
