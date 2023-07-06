using System.Collections.Generic;
using UnityEngine;

namespace Modules.RunnerGame.Scripts.Level.Buff
{
    public class BuffObject : MonoBehaviour
    {
        [SerializeField] private List<Collider> colliders;

        private int _index;
        private GameObject _gameObject;
        private Buff _buff;

        public Buff Buff => _buff;

        public GameObject GameObject => _gameObject;
        
        public Transform Transform => _gameObject.transform;

        public int Index => _index;


        public void Construct(int index, GameObject gameObject, Buff buff)
        {
            _index = index;
            _gameObject = gameObject;
            _buff = buff;

            colliders = new List<Collider>();
            colliders.AddRange(_gameObject.GetComponents<Collider>());
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player.Player player))
            {
                foreach (var collider in colliders)
                {
                    collider.enabled = false;
                }
                
                AddBuff(player);
            }
        }
        
        private void AddBuff(Player.Player player)
        {
            if (_buff is HealthBuff healthBuff)
            {
                player.SetHealthBuff(healthBuff);
            }
            else if (_buff is SpeedBuff speedBuff)
            {
                player.SetSpeedBuff(speedBuff);
            }
            else if (_buff is InvincibleBuff invincibleBuff)
            {
                player.SetInvincibleBuff(invincibleBuff);
            }
        }
    }
}