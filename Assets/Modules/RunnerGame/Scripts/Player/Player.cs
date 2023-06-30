using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Modules.MainModule.Scripts.InputServices;
using Modules.RunnerGame.Scripts.Animation;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Modules.RunnerGame.Scripts.Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private AnimationController animationController;

        [SerializeField] private PlayerConfig playerConfig;

        private PlayerStats playerStats;
        
        private PlayerMovement _playerMovement;
        private PlayerView _playerView;

        private float resetSpeedCurrentDuration;

        private bool isSpeedResetInProgress;

        private CancellationTokenSource _cancellationTokenSource;

        public int Health => playerStats.Health;
        public float Speed => _playerMovement.Speed;
        
        public UnityAction OnDeath;
        public UnityAction<int> OnHealthChange;
        public UnityAction<float> OnSpeedChange;

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

            playerStats = new PlayerStats
            {
                Health = playerConfig.StartHealthPoints
            };
            
            OnHealthChange?.Invoke(playerStats.Health);
        }

        public void FixedUpdate()
        {
            if(GameSettings.IS_PAUSED) return;
            
            _playerMovement?.IsGrounded();
            _playerMovement?.Move();
        }

        public void ReceiveDamage(int damage)
        {
            playerStats.Health -= damage;
            OnHealthChange?.Invoke(playerStats.Health);

            _playerMovement.Speed = 0;
            OnSpeedChange?.Invoke(_playerMovement.Speed);

            _cancellationTokenSource = new CancellationTokenSource();
            if(!isSpeedResetInProgress) ResetSpeed();
            resetSpeedCurrentDuration = 0;
            
            if (playerStats.Health <= 0)
            {
                OnDeath?.Invoke();
                _cancellationTokenSource.Cancel();
            }
        }

        async UniTask ResetSpeed()
        {
            isSpeedResetInProgress = true;
            resetSpeedCurrentDuration = 0;

            var factor = resetSpeedCurrentDuration / playerConfig.ResetSpeedDuration;

            while (factor <= 1)
            {
                _playerMovement.Speed = Mathf.Lerp(0, playerConfig.Speed, factor);
                OnSpeedChange?.Invoke(_playerMovement.Speed);

                resetSpeedCurrentDuration += Time.deltaTime;
                factor = resetSpeedCurrentDuration / playerConfig.ResetSpeedDuration;
                
                await UniTask.WaitForEndOfFrame(this, cancellationToken: _cancellationTokenSource.Token);

                if (playerStats.Health <= 0)
                {
                    break;
                }
            }
            
            isSpeedResetInProgress = false;
        }
    }
}