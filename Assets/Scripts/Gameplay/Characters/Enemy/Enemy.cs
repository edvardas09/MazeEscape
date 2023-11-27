using MazeEscape.Gameplay.State;
using UnityEngine;

namespace MazeEscape.Gameplay.Characters
{
    public class Enemy : Character
    {
        [Header("Damage")]
        [SerializeField] private int m_damage = 10;
        [SerializeField] private float m_damageDistance = 2;
        [SerializeField] private float m_damageCooldown = 2;
        [SerializeField] private int m_damageAngle = 30;
        [SerializeField] private Color m_damageColor = Color.red;

        [Header("View")]
        [SerializeField] private float m_viewDistance = 5;
        [SerializeField] private int m_viewAngle = 90;
        [SerializeField] private Color m_viewColor = Color.green;

        [Header("Components")]
        [SerializeField] private StateMachine m_stateMachine;
        [SerializeField] private ViewCone m_damageCone;
        [SerializeField] private ViewCone m_viewCone;

        private float m_damageCooldownTimer;

        private void Awake()
        {
            m_stateMachine.Setup(m_viewDistance, m_viewAngle);
            m_damageCone.Setup(m_damageDistance, m_damageAngle, m_damageColor);
            m_viewCone.Setup(m_viewDistance, m_viewAngle, m_viewColor);
        }

        private void Update()
        {
            if (m_damageCooldownTimer > 0)
            {
                m_damageCooldownTimer -= Time.deltaTime;
            }
        }
    }
}