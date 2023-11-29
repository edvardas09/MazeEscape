using MazeEscape.Gameplay.State;
using UnityEngine;

namespace MazeEscape.Sounds
{
    public class EnemyAlert : MonoBehaviour
    {
        [SerializeField] private StateMachine m_stateMachine;
        [SerializeField] private AlertFX m_alertFXPrefab;

        private void Awake()
        {
            m_stateMachine.OnStateChange += OnStateChange;
        }

        private void OnStateChange()
        {
            if (m_stateMachine.CurrentState is Alert)
            {
                var position = transform.position;
                position.y += 2f;

                var alertFX = Instantiate(m_alertFXPrefab, position, Quaternion.identity);
                alertFX.transform.SetParent(transform);
            }
        }
    }
}