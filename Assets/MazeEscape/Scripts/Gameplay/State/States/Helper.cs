using System.Collections.Generic;
using MazeEscape.MazeGenerator;
using UnityEngine;
using UnityEngine.AI;
using MazeEscape.Extensions;

namespace MazeEscape.Gameplay.State
{
    public static class Helper
    {
        private const string c_playerTag = "Player";

        public static NavMeshPath GetRandomPath(Transform transform, float radius, List<Cell> cells, NavMeshAgent agent)
        {
            var agentPositionInCoords = transform.position.WorldToCellSpace();
            var currentCell = cells.Find(c => c.Coords == agentPositionInCoords);
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
            agent.CalculatePath(hit.position, newPath);

            return newPath;
        }

        public static bool TryGetTarget(Transform Transform, StateMachine stateMachine, out Vector3 targetPosition)
        {
            targetPosition = Vector3.zero;

            var halfAngle = stateMachine.ViewAngle / 2;

            var frontDistance = stateMachine.ViewDistanceFront;
            var backDistance = stateMachine.ViewDistanceBack;
      
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
                    if (hit.collider.CompareTag(c_playerTag))
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