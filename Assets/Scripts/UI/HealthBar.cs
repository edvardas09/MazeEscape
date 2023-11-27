using MazeEscape.Gameplay.Characters;
using UnityEngine;
using UnityEngine.UI;

namespace MazeEsacpe.UI
{
    public class HealthBar : CanvasBase
    {
        [SerializeField] private Image m_healthBarFill;

        public override void Initialize()
        {
            var player = FindObjectOfType<Player>();
            player.OnHealthChanged += UpdateHealthBar;
        }

        private void UpdateHealthBar(int maxHealth, int currentHealth)
        {
            m_healthBarFill.fillAmount = (float)currentHealth / maxHealth;
        }
    }
}