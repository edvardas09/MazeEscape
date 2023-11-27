using UnityEngine;
using UnityEngine.Events;

namespace MazeEscape.Gameplay.Characters
{
    public class Player : Character
    {
        public UnityAction<int, int> OnHealthChanged;
        public UnityAction OnPlayerDied;

        [SerializeField] private int m_maxHealth = 100;

        private readonly PlayerHealthChangedSignal m_playerHealthChangedSignal;

        private int m_currentHealth;

        public Player(PlayerHealthChangedSignal playerHealthChangedSignal)
        {
            m_playerHealthChangedSignal = playerHealthChangedSignal;
            m_currentHealth = m_maxHealth;
        }

        public void TakeDamage(int damage)
        {
            m_currentHealth -= damage;
            OnHealthChanged?.Invoke(m_maxHealth, m_currentHealth);

            if (m_currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Debug.Log("Player died");
            OnPlayerDied?.Invoke();
        }
    }
}