using UnityEngine;
using UnityEngine.AI;

namespace MazeEscape.Gameplay.State
{
    public class Alert : State
    {
        private const float c_alertTime = 1f;

        private float m_startTime;
        private Vector3 m_targetPosition = Vector3.zero;

        public override void OnExit()
        {
        }

        public override void OnStart()
        {
            m_agent.speed = 0;

            m_ownerStateMachine.Animator.SetBool("IsRunning", false);
            m_ownerStateMachine.Animator.SetBool("IsWalking", false);

            m_startTime = Time.realtimeSinceStartup;
            
            Helper.TryGetTarget(Transform, m_ownerStateMachine, out m_targetPosition);

            if(m_targetPosition != Vector3.zero)
            {
                var path = new NavMeshPath();
                m_agent.CalculatePath(m_targetPosition, path);
                m_agent.SetPath(path);
            }
        }

        public override void OnUpdate()
        {
            if (Time.realtimeSinceStartup - m_startTime > c_alertTime)
            {
                m_ownerStateMachine.SetState<Chasing>();
            }
        }
    }
}