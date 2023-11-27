using MazeEscape.MazeGenerator;
using MazeEscape.MazeGenerator.Room;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MazeEscape.GenerationProcesses
{
    [CreateAssetMenu(fileName = "EntranceGeneration", menuName = "Maze/Generation/EntranceGeneration")]
    public class EntranceGenerationSO : GenerationProcessSO, IEntranceGeneration
    {
        [Header("Generation Settings")]
        [SerializeField] private int m_singlePassageWeight = 1;
        [SerializeField] private int m_doublePassageWeight = 1;
        [SerializeField] private int m_triplePassageWeight = 1;
        [SerializeField] private int m_quadruplePassageWeight = 1;

        [Header("Debug")]
        [SerializeField] private List<Passage> m_passages = new();

        public List<Passage> GetPassages()
        {
            return m_passages;
        }

        public override void OnDrawGizmos()
        {
            if (m_passages.Count == 0)
            {
                return;
            }

            Gizmos.color = new Color(1, 0, 0, 0.5f);
            foreach (var passage in m_passages)
            {
                var entranceCell = passage.Cells[0];
                var exitCell = passage.Cells[1];
                Gizmos.DrawLine(entranceCell.Coords.CellToWorldSpace(1), exitCell.Coords.CellToWorldSpace(1));
            }
        }

        protected override bool OnGenerate()
        {
            m_passages = new();

            if (!TryGetProcess<ICellGeneration>(out var cellGeneration))
            {
                Debug.LogError("EntranceGenerationSO requires a ICellGeneration to be present in the MazeGeneratorSO");
                return false;
            }

            var cells = cellGeneration.GetCells();

            AddAllEntrances(cells);
            RemoveEntrancesByWeights(cells);

            return true;
        }

        private void AddAllEntrances(List<Cell> cells)
        {
            foreach (var cell in cells)
            {
                AddEntranceToCoords(cells, cell, RoomEntranceDirection.Top);
                AddEntranceToCoords(cells, cell, RoomEntranceDirection.Right);
                AddEntranceToCoords(cells, cell, RoomEntranceDirection.Bottom);
                AddEntranceToCoords(cells, cell, RoomEntranceDirection.Left);
            }
        }

        private void AddEntranceToCoords(List<Cell> cells, Cell cell, RoomEntranceDirection roomEntranceDirection)
        {
            var neighborCell = GetCell(cells, cell.Coords + roomEntranceDirection.GetVector());
            if (neighborCell == null)
            {
                return;
            }

            if (m_passages.Any(x => x.Cells.Contains(cell) && x.Cells.Contains(neighborCell)))
            {
                return;
            }

            var passage = new Passage
            {
                Cells = new List<Cell> { cell, neighborCell },
            };

            m_passages.Add(passage);
        }

        private Cell GetCell(List<Cell> cells, Vector2Int coords)
        {
            return cells.FirstOrDefault(x => x.Coords == coords);
        }

        private void RemoveEntrancesByWeights(List<Cell> cells)
        {
            var cellsToModify = cells.Where(x => m_passages.Where(passage => passage.Cells.Contains(x)).Count() > 1).ToList();

            var allWeights = m_singlePassageWeight + m_doublePassageWeight + m_triplePassageWeight + m_quadruplePassageWeight;

            foreach (var cell in cellsToModify)
            {
                var availablePassages = m_passages.Where(passage => passage.Cells.Contains(cell)).ToList();

                var randomWeight = Random.Range(0, allWeights + 1);

                if (randomWeight < m_singlePassageWeight)
                {
                    RemovePassages(cells, availablePassages, 1);
                    continue;
                }
                else if (randomWeight < m_singlePassageWeight + m_doublePassageWeight)
                {
                    RemovePassages(cells, availablePassages, 2);
                    continue;
                }
                else if (randomWeight < m_singlePassageWeight + m_doublePassageWeight + m_triplePassageWeight)
                {
                    RemovePassages(cells, availablePassages, 3);
                    continue;
                }
            }
        }

        private void RemovePassages(List<Cell> cells, List<Passage> availablePassages, int passagesCount)
        {
            var savedPassages = new List<Passage>(m_passages);
            while (availablePassages.Count > passagesCount)
            {
                RemoveRandomPassage(availablePassages);

                if (!ValidatePassages(cells))
                {
                    m_passages = savedPassages;
                    break;
                }
            }
        }

        private void RemoveRandomPassage(List<Passage> availablePassages)
        {
            var randomPassage = availablePassages[Random.Range(0, availablePassages.Count)];
            m_passages.Remove(randomPassage);
            availablePassages.Remove(randomPassage);
        }

        private bool ValidatePassages(List<Cell> cells)
        {
            var currentIterationCells = new List<Cell> { cells[0] };
            var remainingCells = new List<Cell>(cells);

            var iterationCount = cells.Count;
            while (iterationCount > 0)
            {
                iterationCount--;
                var tempCells = Iterate(currentIterationCells, remainingCells);
                if (tempCells.Count == 0)
                {
                    break;
                }

                currentIterationCells = tempCells;
            }

            return remainingCells.Count == 0;
        }

        public List<Cell> Iterate(List<Cell> currentIterationCells, List<Cell> remainingCells)
        {
            var newCells = new List<Cell>();

            for (int i = 0; i < currentIterationCells.Count; i++)
            {
                var cell = currentIterationCells[i];
                remainingCells.Remove(cell);

                IteratePassages(cell, newCells, remainingCells);
            }

            return newCells;
        }

        private void IteratePassages(Cell cell, List<Cell> newCells, List<Cell> remainingCells)
        {
            var availablePassages = m_passages.Where(p => p.Cells.Contains(cell)).ToList();
            if (availablePassages.Count == 0)
            {
                return;
            }

            availablePassages = availablePassages.Where(x => remainingCells.Contains(x.Cells.First(x => x != cell))).ToList();
            availablePassages = availablePassages.Where(x => !newCells.Contains(x.Cells.First(x => x != cell))).ToList();

            newCells.AddRange(availablePassages.Select(x => x.Cells.First(x => x != cell)).ToList());
        }
    }
}