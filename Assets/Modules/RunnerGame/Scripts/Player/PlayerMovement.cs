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
        private int jumpsCount;

        public UnityAction OnGrounded;
        public UnityAction OnJump;
        
        public float Speed
        {
            get => speed;
            set => speed = value;
        }

        public PlayerMovement(InputService inputService, Rigidbody rigidbody, Transform transform, float speed, float jumpForce)
        {
            this.inputService = inputService;
            this.rigidbody = rigidbody;
            this.transform = transform;

            this.speed = speed;
            this.jumpForce = jumpForce;

            this.inputService.OnTap += Jump;
        
            SetMoveDirectionZ(1f);
        }

        private void Jump()
        {
            if (isGrounded || jumpsCount < 1)
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

        public void IsGrounded()
        {
            var startPos = transform.position;
            var direction = startPos - transform.up * 0.05f;

            Physics.Raycast(startPos, direction, out var hitInfo);
            Debug.DrawLine(startPos, direction, Color.magenta);
            
            isGrounded = hitInfo.transform != null;
        }

        private void Gravity()
        {
            rigidbody.AddForce(new Vector3(0, -9.81f, 0));
        }
    }
}
