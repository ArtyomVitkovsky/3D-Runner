using System.Threading;
using Cysharp.Threading.Tasks;
using Modules.MainModule.Scripts.InputServices;
using Modules.RunnerGame.Scripts.Animation;
using Modules.RunnerGame.Scripts.Level.Buff;
using Modules.RunnerGame.Scripts.Level.Platform;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Modules.RunnerGame.Scripts.Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private AnimationController animationController;
        [SerializeField] private LayerMask groundMask;

        [SerializeField] private PlayerConfig playerConfig;

        private PlayerStats _playerStats;

        [SerializeField] private PlayerMovement _playerMovement;
        private PlayerView _playerView;

        private float _resetSpeedCurrentDuration;

        private bool _isSpeedResetInProgress;

        private CancellationTokenSource _cancellationTokenSource;

        private HealthBuff _healthBuff;
        private SpeedBuff _speedBuff;
        private InvincibleBuff _invincibleBuff;

        public int Health
        {
            get
            {
                var value = _playerStats.Health;
                if (_healthBuff != null) value += _healthBuff.Value;
                return value;
            }
        }

        public float Speed
        {
            get
            {
                var value = _playerMovement.Speed;
                if (_speedBuff != null) value += _speedBuff.Value;
                return value;
            }
        }

        public UnityAction<Platform> OnDeath;
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
                playerConfig.JumpForce,
                groundMask);

            _playerView = new PlayerView(animationController);

            _playerMovement.OnJump += _playerView.OnJump;
            _playerMovement.OnGrounded += _playerView.OnRun;
            OnSpeedChange += _playerMovement.SetSpeed;

            _playerStats = new PlayerStats
            {
                Health = playerConfig.StartHealthPoints
            };

            OnHealthChange?.Invoke(Health);
        }

        private void OnBuffEnded(Buff buff)
        {
            if (buff is HealthBuff healthBuff)
            {
                healthBuff.Value = 0;
            }
            else if (buff is SpeedBuff speedBuff)
            {
                speedBuff.Value = 0;
            }
        }

        public void FixedUpdate()
        {
            if (GameSettings.IS_PAUSED)
            {
                _playerMovement?.Stop();
                return;
            }

            _playerMovement?.IsGrounded();
            _playerMovement?.Move();
        }

        private void Update()
        {
            _healthBuff?.CheckDuration();
            _speedBuff?.CheckDuration();
            _invincibleBuff?.CheckDuration();
        }

        public void ReceiveDamage(int damage, Platform platform)
        {
            if (_invincibleBuff is {IsActive: true}) return;

            if (_healthBuff is {Value: > 0})
            {
                _healthBuff.Value -= damage;
            }
            else
            {
                _playerStats.Health -= damage;
            }

            OnHealthChange?.Invoke(Health);

            _playerMovement.SetSpeed(0);
            if (_speedBuff != null) _speedBuff.Value = 0;
            
            OnSpeedChange?.Invoke(Speed);

            _cancellationTokenSource = new CancellationTokenSource();
            if (!_isSpeedResetInProgress) ResetSpeed();
            _resetSpeedCurrentDuration = 0;

            if (_playerStats.Health <= 0)
            {
                OnDeath?.Invoke(platform);
                _cancellationTokenSource.Cancel();
            }
        }

        async UniTask ResetSpeed()
        {
            _isSpeedResetInProgress = true;
            _resetSpeedCurrentDuration = 0;

            var factor = _resetSpeedCurrentDuration / playerConfig.ResetSpeedDuration;

            while (factor <= 1)
            {
                _playerMovement.SetSpeed(Mathf.Lerp(0, playerConfig.Speed, factor));
                OnSpeedChange?.Invoke(_playerMovement.Speed);

                _resetSpeedCurrentDuration += Time.deltaTime;
                factor = _resetSpeedCurrentDuration / playerConfig.ResetSpeedDuration;

                await UniTask.WaitForEndOfFrame(this, cancellationToken: _cancellationTokenSource.Token);

                if (_playerStats.Health <= 0)
                {
                    break;
                }
            }

            _isSpeedResetInProgress = false;
        }

        private void OnDestroy()
        {
            _playerMovement.OnJump -= _playerView.OnJump;
            _playerMovement.OnGrounded -= _playerView.OnRun;

            _playerMovement.OnDestroy();
        }

        public void SetHealthBuff(HealthBuff healthBuff)
        {
            _healthBuff = new HealthBuff(healthBuff.Duration, healthBuff.Value);
            OnHealthChange?.Invoke(Health);
            _healthBuff.OnBuffEnded += OnBuffEnded;
        }

        public void SetSpeedBuff(SpeedBuff speedBuff)
        {
            _speedBuff = new SpeedBuff(speedBuff.Duration, speedBuff.Value);
            _playerMovement.SetSpeed(Speed);
            OnSpeedChange?.Invoke(Speed);
            _speedBuff.OnBuffEnded += OnBuffEnded;
        }

        public void SetInvincibleBuff(InvincibleBuff invincibleBuff)
        {
            _invincibleBuff = new InvincibleBuff(invincibleBuff.Duration);
        }

        public void Reset()
        {
            _playerMovement.SetSpeed(playerConfig.Speed);
            _playerStats.Health = playerConfig.StartHealthPoints;
        }
    }
}