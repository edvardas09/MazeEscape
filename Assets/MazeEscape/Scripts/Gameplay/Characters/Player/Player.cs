using UnityEngine;
using UnityEngine.Events;

namespace MazeEscape.Gameplay.Characters
{
    public class Player : Character
    {
        public UnityAction<float, float> OnHealthChanged;
        public UnityAction OnPlayerDied;

        [SerializeField] private float m_maxHealth = 100;

        private float m_currentHealth;

        private void Awake()
        {
            m_currentHealth = m_maxHealth;
        }

        public void TakeDamage(float damage)
        {
            m_currentHealth -= damage;
            OnHealthChanged?.Invoke(m_maxHealth, m_currentHealth);

            if (m_currentHealth <= 0)
            {
                OnPlayerDied?.Invoke();
            }
        }
    }
}