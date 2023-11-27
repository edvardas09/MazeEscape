using System.Collections.Generic;
using MazeEscape.MazeGenerator;
using UnityEngine;
using UnityEngine.AI;

namespace MazeEscape.Gameplay.State
{
    public class Patrolling : State
    {
        private const string PlayerTag = "Player";

        private const float k_PatrolRadius = 5f;
        private const float k_minDistanceToTarget = 0.2f;

        private List<Cell> m_cells;

        public override void OnExit()
        {
            m_agent.ResetPath();
        }

        public override void OnStart()
        {
            if (!m_ownerStateMachine.MazeGenerator.TryGetProcess<ICellGeneration>(out var cellGeneration))
            {
                Debug.LogError("Patrolling state requires a ICellGeneration to be present in the MazeGeneratorSO");
                return;
            }

            m_cells = cellGeneration.GetCells();

            var path = GetRandomPath(k_PatrolRadius);
            m_agent.SetPath(path);
        }

        public override void OnUpdate()
        {
            if (!TryGetTarget(out var targetPosition))
            {
                if (m_agent.remainingDistance < k_minDistanceToTarget)
                {
                    var randomPath = GetRandomPath(k_PatrolRadius);
                    m_agent.SetPath(randomPath);
                }

                return;
            }

            var path = new NavMeshPath();
            m_agent.CalculatePath(targetPosition, path);
            m_agent.SetPath(path);
        }

        private NavMeshPath GetRandomPath(float radius)
        {
            var agentPositionInCoords = Transform.position.WorldToCellSpace();
            var currentCell = m_cells.Find(c => c.Coords == agentPositionInCoords);
            if (currentCell == null)
            {
                Debug.LogError("Couldn't find the current cell");
                return null;
            }

            var randomDirection = Random.insideUnitSphere * radius;
            randomDirection += currentCell.Coords.CellToWorldSpace();

            if (!NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, 1))
            {
                Debug.LogError("Couldn't find a random position");
            }

            var newPath = new NavMeshPath();
            m_agent.CalculatePath(hit.position, newPath);

            return newPath;
        }

        private bool TryGetTarget(out Vector3 targetPosition)
        {
            targetPosition = Vector3.zero;

            var halfAngle = m_ownerStateMachine.ViewAngle / 2;

            var frontDistance = m_ownerStateMachine.ViewDistanceFront;
            var backDistance = m_ownerStateMachine.ViewDistanceBack;
      
            for (var i = -180; i < 180; i++)
            {
                var distanceByAngle = Mathf.Abs(i) <= halfAngle ? frontDistance : 0;
                distanceByAngle = Mathf.Max(backDistance, distanceByAngle);

                var getYTransformAngle  = Transform.rotation.eulerAngles.y + i;
                var direction = Quaternion.Euler(0, getYTransformAngle, 0) * Vector3.forward;
                var ray = new Ray(Transform.position, direction);

                if (Physics.Raycast(ray, out var hit, distanceByAngle))
                {
#if UNITY_EDITOR
                    Debug.DrawLine(ray.origin, hit.point, Color.green);
#endif
                    if (hit.collider.CompareTag(PlayerTag))
                    {
                        
                        targetPosition = hit.collider.transform.position;
                    }
                }
                else
                {
#if UNITY_EDITOR
                Debug.DrawRay(ray.origin, ray.direction * distanceByAngle, Color.red);
#endif
                }
            }
            
            return targetPosition != Vector3.zero;
        }
    }
}