using UnityEngine;

namespace Modules.RunnerGame.Scripts.Level.Platform
{
    public class Damager : MonoBehaviour
    {
        [SerializeField] private Collider[] colliders;
        [SerializeField] private int damage;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player.Player player))
            {
                foreach (var collider in colliders)
                {
                    collider.enabled = false;
                }
                player.ReceiveDamage(damage);
                
                Debug.LogWarning($"DAMAGE BY {name} FOR {damage} POINTS");
            }
        }
    }
}