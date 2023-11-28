using MazeEscape.Gameplay.State;
using UnityEngine;

namespace MazeEscape.Gameplay.Characters
{
    public class Enemy : Character
    {
        [Header("Movement")]
        [SerializeField] private float m_walkSpeed = 1.5f;
        [SerializeField] private float m_runSpeed = 3f;

        [Header("Damage")]
        [SerializeField] private float m_damage = 1f;
        [SerializeField] private float m_damageDistance = 2;
        [SerializeField] private int m_damageAngle = 30;
        [SerializeField] private Color m_damageColor = Color.red;

        [Header("View")]
        [SerializeField] private float m_viewDistance = 5;
        [SerializeField] private int m_viewAngle = 90;
        [SerializeField] private Color m_viewColor = Color.green;
        [SerializeField] private Color m_seenColor = Color.yellow;

        [Header("Components")]
        [SerializeField] private StateMachine m_stateMachine;
        [SerializeField] private ViewCone m_damageCone;
        [SerializeField] private ViewCone m_viewCone;

        private Player m_player;

        public float WalkSpeed => m_walkSpeed;
        public float RunSpeed => m_runSpeed;

        private void Awake()
        {
            m_stateMachine.Setup(m_viewDistance, m_viewAngle);
            m_damageCone.Setup(m_damageDistance, m_damageAngle, m_damageColor);
            m_viewCone.Setup(m_viewDistance, m_viewAngle, m_viewColor);

            m_damageCone.OnPlayerFound += OnPlayerFound;
            m_damageCone.OnPlayerLost += OnPlayerLost;

            m_stateMachine.OnStateChange += OnStateChange;
        }

        private void OnStateChange()
        {
            if (m_stateMachine.CurrentState is Chasing)
            {
                m_viewCone.Setup(m_viewDistance, m_viewAngle, m_seenColor);
            }
            else
            {
                m_viewCone.Setup(m_viewDistance, m_viewAngle, m_viewColor);
            }
        }

        private void OnDestroy()
        {
            m_damageCone.OnPlayerFound -= OnPlayerFound;
            m_damageCone.OnPlayerLost -= OnPlayerLost;

            m_stateMachine.OnStateChange -= OnStateChange;
        }

        private void OnPlayerLost(GameObject playerObject)
        {
            m_player = null;
        }

        private void OnPlayerFound(GameObject playerObject)
        {
            if (!playerObject.TryGetComponent<Player>(out var player))
            {
                Debug.LogError("Player does not have a Player component");
                return;
            }

            m_player = player;
        }

        private void FixedUpdate()
        {
            if (!m_player)
            {
                return;
            }

            m_player.TakeDamage(m_damage);
        }
    }
}