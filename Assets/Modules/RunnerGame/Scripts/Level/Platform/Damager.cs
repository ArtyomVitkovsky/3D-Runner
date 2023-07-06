using UnityEngine;

namespace Modules.RunnerGame.Scripts.Level.Platform
{
    public class Damager : MonoBehaviour
    {
        [SerializeField] private Collider[] colliders;
        [SerializeField] private int damage;

        [SerializeField] private Platform platform;
        
        public void SetPlatform(Platform platform)
        {
            this.platform = platform;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player.Player player))
            {
                foreach (var collider in colliders)
                {
                    collider.enabled = false;
                }
                player.ReceiveDamage(damage, platform);
            }
        }
    }
}