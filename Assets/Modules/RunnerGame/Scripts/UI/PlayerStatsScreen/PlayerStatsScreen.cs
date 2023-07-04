using Modules.MainModule.Scripts.UI.Interfaces;
using TMPro;
using UnityEngine;

namespace Modules.RunnerGame.Scripts.UI
{
    public class PlayerStatsScreen : MonoBehaviour, IScreen
    {
        [SerializeField] private TextMeshProUGUI health;
        [SerializeField] private TextMeshProUGUI speed;

        public void UpdateHealthText(int health)
        {
            this.health.text = $"{health}";
        }
        
        public void UpdateSpeedText(float speed)
        {
            this.speed.text = $"{speed:0.##}";
        }

        public void Initialize()
        {
            
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}