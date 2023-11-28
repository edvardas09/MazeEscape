using MazeEscape.MazeGenerator;
using UnityEngine;
using MazeEscape.Extensions;
using Zenject;
using System.Linq;

namespace MazeEscape.GenerationProcesses
{
    [CreateAssetMenu(fileName = "StartVisualGeneration", menuName = "Maze/Generation/StartVisualGeneration")]
    public class StartVisualGenerationSO : GenerationProcessSO
    {
        [SerializeField] private GameObject m_startPrefab;
        [SerializeField] private GameObject m_start;

        private IStartCell m_startCellGeneration;

        [Inject]
        public void Initialize(IStartCell startCellGeneration)
        {
            m_startCellGeneration = startCellGeneration;
        }

        public override bool IsCompleted()
        {
            // Always regenerate visual assets
            return false;
        }

        public override void Reset()
        {
            if (m_start)
            {
                DestroyImmediate(m_start);
            }

            m_start = null;
        }

        protected override bool OnGenerate()
        {
            var startCells = m_startCellGeneration.GetStartCells();
            m_start = Instantiate(m_startPrefab, startCells.First().Coords.CellToWorldSpace(), Quaternion.identity);
            m_start.transform.parent = m_mazeGenerator.transform;

            return true;
        }
    }
}