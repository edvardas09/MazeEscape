using System.Collections.Generic;
using MazeEscape.MazeGenerator;
using UnityEngine;
using UnityEngine.AI;

namespace MazeEscape.Gameplay.State
{
    public class Patrolling : State
    {
        private const float c_PatrolRadius = 5f;
        private const float c_minDistanceToTarget = 0.2f;

        private List<Cell> m_cells;

        public override void OnExit()
        {
            m_agent.ResetPath();

            m_ownerStateMachine.Animator.SetBool("IsWalking", false);
        }

        public override void OnStart()
        {
            if (!m_ownerStateMachine.MazeGenerator.TryGetProcess<ICellGeneration>(out var cellGeneration))
            {
                Debug.LogError("Patrolling state requires a ICellGeneration to be present in the MazeGeneratorSO");
                return;
            }

            m_cells = cellGeneration.GetCells();

            var path = Helper.GetRandomPath(Transform, c_PatrolRadius, m_cells, m_agent);
            m_agent.SetPath(path);

            m_agent.speed = m_ownerStateMachine.Enemy.WalkSpeed;

            m_ownerStateMachine.Animator.SetBool("IsWalking", true);
            m_ownerStateMachine.Animator.SetTrigger("StartWalking");        
        }

        public override void OnUpdate()
        {
            if (!Helper.TryGetTarget(Transform, m_ownerStateMachine, out var targetPosition))
            {
                if (m_agent.remainingDistance < c_minDistanceToTarget)
                {
                    var randomPath = Helper.GetRandomPath(Transform, c_PatrolRadius, m_cells, m_agent);
                    m_agent.SetPath(randomPath);
                }

                return;
            }

            m_ownerStateMachine.SetState<Chasing>();

            var path = new NavMeshPath();
            m_agent.CalculatePath(targetPosition, path);
            m_agent.SetPath(path);
        }
    }
}