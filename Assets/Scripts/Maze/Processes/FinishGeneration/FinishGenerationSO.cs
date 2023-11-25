using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MazeEscape.Maze
{
    [CreateAssetMenu(fileName = "FinishGeneration", menuName = "Maze/Generation/FinishGeneration")]
    public class FinishGenerationSO : GenerationProcessSO, IFinishCell
    {
        [SerializeField] private Cell m_finishCell;

        private List<Cell> m_remainingCells = new List<Cell>();
        private List<Cell> m_currentIterationCells = new List<Cell>();
        private List<Passage> m_passages = new List<Passage>();

        public Cell GetFinishCell()
        {
            return m_finishCell;
        }

        public override void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            foreach (var cell in m_currentIterationCells)
            {
                Gizmos.DrawWireCube(new Vector3(cell.Coords.x, 0, cell.Coords.y), new Vector3(.5f, .5f, .5f));
            }
        }

        protected override void OnGenerate()
        {
            m_currentIterationCells.Clear();
            m_remainingCells.Clear();

            var cellGeneration = GetProcess<ICellGeneration>();
            var cells = cellGeneration.GetCells();
            m_remainingCells.AddRange(cells);

            var passageGeneration = GetProcess<IEntranceGeneration>();
            m_passages = passageGeneration.GetPassages();

            var startCell = GetProcess<IStartCell>().GetStartCell();
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
        }

        public List<Cell> Iterate()
        {
            var newCells = new List<Cell>();

            for (int i = 0; i < m_currentIterationCells.Count; i++)
            {
                var cell = m_currentIterationCells[i];
                m_remainingCells.Remove(cell);

                var availablePassages = m_passages.Where(p => p.EntranceCell == cell).ToList();
                if (availablePassages.Count == 0)
                {
                    continue;
                }

                availablePassages = availablePassages.Where(x => m_remainingCells.Contains(x.ExitCell)).ToList();
                availablePassages = availablePassages.Where(x => !newCells.Contains(x.ExitCell)).ToList();

                newCells.AddRange(availablePassages.Select(x => x.ExitCell).ToList());
            }

            return newCells;
        }
    }
}