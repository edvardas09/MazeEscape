using MazeEscape.Gameplay.Characters;
using UnityEngine;
using UnityEngine.UI;

namespace MazeEsacpe.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image m_healthBarFill;

        private void Start()
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