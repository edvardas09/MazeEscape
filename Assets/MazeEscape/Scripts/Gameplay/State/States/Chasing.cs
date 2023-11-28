using UnityEngine.AI;

namespace MazeEscape.Gameplay.State
{
    public class Chasing : State
    {
        private const float c_minDistanceToTarget = 0.2f;

        public override void OnExit()
        {
            m_agent.ResetPath();

            m_ownerStateMachine.Animator.SetBool("IsRunning", false);
        }

        public override void OnStart()
        {
            m_agent.speed = m_ownerStateMachine.Enemy.RunSpeed;

            m_ownerStateMachine.Animator.SetBool("IsRunning", true);
            m_ownerStateMachine.Animator.SetTrigger("StartRunning");     
        }

        public override void OnUpdate()
        {
            if (Helper.TryGetTarget(Transform, m_ownerStateMachine, out var targetPosition))
            {
                var path = new NavMeshPath();
                m_agent.CalculatePath(targetPosition, path);
                m_agent.SetPath(path);
                return;
            }

            if (m_agent.remainingDistance < c_minDistanceToTarget)
            {
                m_ownerStateMachine.SetState<Confused>();
            }    
        }
    }
}