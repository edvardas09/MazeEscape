using MazeEscape.MazeGenerator;
using UnityEngine;

namespace MazeEscape.GenerationProcesses
{
    [CreateAssetMenu(fileName = "NavMeshGeneration", menuName = "Maze/Generation/NavMeshGeneration")]
    public class NavMeshGenerationSO : GenerationProcessSO
    {
        public override bool IsCompleted()
        {
            // Always rebuild nav mesh
            return false;
        }

        public override void Reset()
        {
        }

        protected override bool OnGenerate()
        {
            m_mazeGenerator.NavMeshSurface.BuildNavMesh();
            return true;
        }
    }
}