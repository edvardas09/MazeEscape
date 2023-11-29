
using UnityEngine;
using MazeEscape.Gameplay.State;

namespace MazeEscape.Sounds
{
    public class AlertSound : MonoBehaviour
    {
        [SerializeField] private StateMachine m_stateMachine;
        [SerializeField] private AudioSource m_audioSource;

        private void Awake()
        {
            m_stateMachine.OnStateChange += OnStateChange;
        }

        private void OnDestroy()
        {
            m_stateMachine.OnStateChange -= OnStateChange;
        }

        private void OnStateChange()
        {
            if (m_stateMachine.CurrentState is Alert) 
            {
                m_audioSource.Play();
            }
        }
    }
}
