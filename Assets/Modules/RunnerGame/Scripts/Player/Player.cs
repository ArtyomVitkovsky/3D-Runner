using System;
using Modules.MainModule.Scripts.InputServices;
using Modules.RunnerGame.Scripts.Animation;
using UnityEngine;
using Zenject;

namespace Modules.RunnerGame.Scripts.Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private AnimationController animationController;

        [SerializeField] private PlayerConfig playerConfig;
        
        private PlayerMovement _playerMovement;
        private PlayerView _playerView;

        [Inject]
        private void Construct(InputService inputService)
        {
            _playerMovement = new PlayerMovement(
                inputService, 
                rigidbody, 
                transform,
                playerConfig.Speed,
                playerConfig.JumpForce);
            
            _playerView = new PlayerView(animationController);

            _playerMovement.OnJump += _playerView.OnJump;
            _playerMovement.OnGrounded += _playerView.OnRun;
        }

        private void FixedUpdate()
        {
            _playerMovement?.IsGrounded();
            _playerMovement?.Move();
        }
    }
}