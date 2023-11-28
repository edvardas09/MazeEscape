using MazeEscape.MazeGenerator;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MazeEscape.Extensions;
using Zenject;

namespace MazeEscape.GenerationProcesses
{
    [CreateAssetMenu(fileName = "FinishGeneration", menuName = "Maze/Generation/FinishGeneration")]
    public class FinishGenerationSO : GenerationProcessSO, IFinishCell
    {
        [SerializeField] private List<Cell> m_finishCells = new();

        private List<Cell> m_remainingCells = new();
        private List<Cell> m_currentIterationCells = new();
        private List<Passage> m_passages = new();

        private ICellGeneration m_cellGeneration;
        private IEntranceGeneration m_entranceGeneration;
        private IStartCell m_startCellGeneration;

        [Inject]
        public void Initialize(ICellGeneration cellGeneration, IEntranceGeneration entranceGeneration, IStartCell startCellGeneration)
        {
            m_cellGeneration = cellGeneration;
            m_entranceGeneration = entranceGeneration;
            m_startCellGeneration = startCellGeneration;
        }

        public override bool IsCompleted()
        {
            return m_finishCells.Count > 0;
        }

        public override void Reset()
        {
            m_finishCells.Clear();
            m_remainingCells.Clear();
            m_currentIterationCells.Clear();
        }

        public List<Cell> GetFinishCells()
        {
            return m_finishCells;
        }

        public override void OnDrawGizmos()
        {
            if (m_finishCells == null)
            {
                return;
            }

            Gizmos.color = new Color(0, 0, 1, 0.5f);

            foreach (var cell in m_finishCells)
            {
                Gizmos.DrawCube(new Vector3(cell.Coords.x, 0, cell.Coords.y), new Vector3(1, 1, 1));
            }
        }

        protected override bool OnGenerate()
        {
            m_currentIterationCells.Clear();
            m_remainingCells.Clear();

            var cells = m_cellGeneration.GetCells();
            m_remainingCells.AddRange(cells);
            m_passages = m_entranceGeneration.GetPassages();

            var startCells = m_startCellGeneration.GetStartCells();
            m_currentIterationCells.Add(startCells.First());

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

            m_finishCells.Add(m_currentIterationCells[Random.Range(0, m_currentIterationCells.Count)]);

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