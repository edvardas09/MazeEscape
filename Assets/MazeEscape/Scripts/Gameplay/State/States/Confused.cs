using UnityEngine;

namespace MazeEscape.Gameplay.State
{
    public class Confused : State
    {
        private const float c_confusedTime = 3f;
        private Quaternion m_startRotation;
        private float m_startTime;

        public override void OnExit()
        {
            
        }

        public override void OnStart()
        {
            m_agent.ResetPath();
            m_ownerStateMachine.Animator.SetBool("IsRunning", false);

            m_startRotation = m_agent.transform.rotation;

            m_startTime = Time.time;
        }

        public override void OnUpdate()
        {
            m_agent.transform.rotation = m_startRotation * Quaternion.Euler(0, Mathf.Sin(Time.time * 3) * 45, 0);

            if (Helper.TryGetTarget(Transform, m_ownerStateMachine, out var _))
            {
                m_ownerStateMachine.SetState<Chasing>();
            }

            if (Time.time - m_startTime > c_confusedTime)
            {
                m_ownerStateMachine.SetState<Patrolling>();
            }
        }
    }
}