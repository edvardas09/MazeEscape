using UnityEngine;

namespace MazeEscape.Gameplay.Characters
{
    public class Player : Character
    {
        [SerializeField] private int m_maxHealth = 100;

        private int m_currentHealth;

        private void Awake()
        {
            m_currentHealth = m_maxHealth;
        }

        public void TakeDamage(int damage)
        {
            m_currentHealth -= damage;
            if (m_currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Debug.Log("Player died");
        }
    }
}