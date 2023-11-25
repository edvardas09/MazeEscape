using System.Collections.Generic;
using UnityEngine;

namespace MazeEscape.Maze
{
    public class MazeGenerator : MonoBehaviour
    {
        [SerializeField] private List<GenerationProcessSO> m_generationProcesses = new();   

        public List<GenerationProcessSO> GenerationProcesses => m_generationProcesses;

        public void Awake()
        {
            GenerateMaze();
        }

        [ContextMenu("Generate Maze")]
        public void GenerateMaze()
        {
            foreach (var process in m_generationProcesses)
            {
                process.Generate(this);
            }
        }

        void OnDrawGizmos()
        {
            foreach (var process in m_generationProcesses)
            {
                process.OnDrawGizmos();
            }
        }
    }
}