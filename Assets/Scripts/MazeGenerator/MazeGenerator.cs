using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

namespace MazeEscape.MazeGenerator
{
    public class MazeGenerator : MonoBehaviour
    {
        [SerializeField] private NavMeshSurface m_navMeshSurface;  
        [SerializeField] private List<GenerationProcessSO> m_generationProcesses = new();

        public List<GenerationProcessSO> GenerationProcesses => m_generationProcesses;

        private void Awake()
        {
            ClearMaze();
            GenerateMaze();
        }

        public bool TryGetProcess<T>(out T process) where T : class
        {
            process = GenerationProcesses.Find(process => process is T) as T;
            return process != null;
        }

        [ContextMenu("Generate Maze")]
        private void GenerateMaze()
        {
            ClearMaze();

            foreach (var process in m_generationProcesses)
            {
                if (!process.Generate(this))
                {
                    Debug.LogError($"Failed to generate {process.name}");
                    return;
                }
            }

            m_navMeshSurface.BuildNavMesh();
        }

        [ContextMenu("Clear Maze")]
        private void ClearMaze()
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