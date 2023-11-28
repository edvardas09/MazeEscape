using MazeEscape.MazeGenerator;
using UnityEngine;
using MazeEscape.Extensions;
using Zenject;
using System.Linq;

namespace MazeEscape.GenerationProcesses
{
    [CreateAssetMenu(fileName = "FinishVisualGeneration", menuName = "Maze/Generation/FinishVisualGeneration")]
    public class FinishVisualGenerationSO : GenerationProcessSO
    {
        [SerializeField] private GameObject m_finishPrefab;
        [SerializeField] private GameObject m_finish;

        private IFinishCell m_finishCellGeneration;

        [Inject]
        public void Initialize(IFinishCell finishCellGeneration)
        {
            m_finishCellGeneration = finishCellGeneration;
        }

        public override bool IsCompleted()
        {
            // Always regenerate visual assets
            return false;
        }

        public override void Reset()
        {
            if (m_finish)
            {
                DestroyImmediate(m_finish);
            }
            
            m_finish = null;
        }

        protected override bool OnGenerate()
        {
            var finishCells = m_finishCellGeneration.GetFinishCells();
            m_finish = Instantiate(m_finishPrefab, finishCells.First().Coords.CellToWorldSpace(), Quaternion.identity);
            m_finish.transform.parent = m_mazeGenerator.transform;
            return true;
        }
    }
}