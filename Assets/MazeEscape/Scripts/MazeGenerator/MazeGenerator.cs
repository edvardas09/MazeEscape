using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

namespace MazeEscape.MazeGenerator
{
    public class MazeGenerator : MonoBehaviour
    {
        [SerializeField, Tooltip("When FALSE, will use existing generated data")] private bool m_forceRegenerate;
        [SerializeField] private NavMeshSurface m_navMeshSurface;  
        [SerializeField] private List<GenerationProcessSO> m_generationProcesses = new();

        public List<GenerationProcessSO> GenerationProcesses => m_generationProcesses;
        public NavMeshSurface NavMeshSurface => m_navMeshSurface;
        public bool ForceRegenerate => m_forceRegenerate;

        private void Awake()
        {
            if (m_forceRegenerate)
            {
                ClearMaze();
            }
        
            GenerateMaze();
        }

        public bool TryGetProcess<T>(out T process) where T : class
        {
            process = GenerationProcesses.Find(process => process is T) as T;
            return process != null;
        }

        [ContextMenu("Regenerate Maze")]
        private void RegenerateMaze()
        {
            ClearMaze();
            GenerateMaze();
        }

        private void GenerateMaze()
        {
            ClearVisuals();
            
            foreach (var process in m_generationProcesses)
            {
                if (process.IsCompleted() && !m_forceRegenerate)
                {
                    Debug.Log($"Using already generated data for {process.name}");
                    continue;
                }

                process.Reset();

                var startTime = Time.realtimeSinceStartup;
                
                if (!process.Generate(this))
                {
                    Debug.LogError($"Failed to generate {process.name}");
                    return;
                }

                var milliseconds = (int)((Time.realtimeSinceStartup - startTime) * 1000);
                Debug.Log($"Generated {process.name} in {milliseconds} ms");
            }
        }

        [ContextMenu("Clear Maze")]
        private void ClearMaze()
        {
            foreach (var process in m_generationProcesses)
            {
                process.Reset();
            }

            ClearVisuals();
        }

        private void ClearVisuals()
        {
            var childrenGameObjects = new List<GameObject>();
            foreach (Transform child in transform)
            {
                childrenGameObjects.Add(child.gameObject);
            }

            foreach (var gameObject in childrenGameObjects)
            {
                DestroyImmediate(gameObject);
            }
        }

        private void OnDrawGizmos()
        {
            foreach (var process in m_generationProcesses)
            {
                process.OnDrawGizmos();
            }
        }
    }
}