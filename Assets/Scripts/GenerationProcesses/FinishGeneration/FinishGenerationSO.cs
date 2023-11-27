using MazeEscape.MazeGenerator;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MazeEscape.GenerationProcesses
{
    [CreateAssetMenu(fileName = "FinishGeneration", menuName = "Maze/Generation/FinishGeneration")]
    public class FinishGenerationSO : GenerationProcessSO, IFinishCell
    {
        [SerializeField] private Cell m_finishCell;

        private List<Cell> m_remainingCells = new();
        private List<Cell> m_currentIterationCells = new();
        private List<Passage> m_passages = new();

        public Cell GetFinishCell()
        {
            return m_finishCell;
        }

        public override void OnDrawGizmos()
        {
            if (m_finishCell == null)
            {
                return;
            }

            Gizmos.color = new Color(0, 0, 1, 0.5f);
            Gizmos.DrawWireCube(m_finishCell.Coords.CellToWorldSpace(), new Vector3(10, 10, 10));
        }

        protected override bool OnGenerate()
        {
            m_currentIterationCells.Clear();
            m_remainingCells.Clear();

            if (!TryGetProcess<ICellGeneration>(out var cellGeneration))
            {
                Debug.LogError("FinishGenerationSO requires a ICellGeneration to be present in the MazeGeneratorSO");
                return false;
            }

            if (!TryGetProcess<IEntranceGeneration>(out var passageGeneration))
            {
                Debug.LogError("FinishGenerationSO requires a IEntranceGeneration to be present in the MazeGeneratorSO");
                return false;
            }

            if (!TryGetProcess<IStartCell>(out var startCellGeneration))
            {
                Debug.LogError("FinishGenerationSO requires a IStartCell to be present in the MazeGeneratorSO");
                return false;
            }

            var cells = cellGeneration.GetCells();
            m_remainingCells.AddRange(cells);
            m_passages = passageGeneration.GetPassages();

            var startCell = startCellGeneration.GetStartCell();
            m_currentIterationCells.Add(startCell);

            var iterationCount = cells.Count;
            while (iterationCount > 0)
            {
                iterationCount--;
                var tempCells = Iterate();
                if (tempCells.Count == 0)
                {
                    break;
                }

                m_currentIterationCells = tempCells;
            }

            m_finishCell = m_currentIterationCells[Random.Range(0, m_currentIterationCells.Count)];

            return true;
        }

        public List<Cell> Iterate()
        {
            var newCells = new List<Cell>();

            for (int i = 0; i < m_currentIterationCells.Count; i++)
            {
                var cell = m_currentIterationCells[i];
                m_remainingCells.Remove(cell);

                IteratePassages(cell, newCells);
            }

            return newCells;
        }

        private void IteratePassages(Cell cell, List<Cell> newCells)
        {
            var availablePassages = m_passages.Where(p => p.Cells.Contains(cell)).ToList();
            if (availablePassages.Count == 0)
            {
                return;
            }

            availablePassages = availablePassages.Where(x => m_remainingCells.Contains(x.Cells.First(x => x != cell))).ToList();
            availablePassages = availablePassages.Where(x => !newCells.Contains(x.Cells.First(x => x != cell))).ToList();

            newCells.AddRange(availablePassages.Select(x => x.Cells.First(x => x != cell)).ToList());
        }
    }
}