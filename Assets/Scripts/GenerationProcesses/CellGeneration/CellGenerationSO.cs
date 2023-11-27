using MazeEscape.MazeGenerator;
using MazeEscape.MazeGenerator.Room;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MazeEscape.GenerationProcesses
{
    [CreateAssetMenu(fileName = "CellGeneration", menuName = "Maze/Generation/CellGeneration")]
    public class CellGeneration : GenerationProcessSO, ICellGeneration
    {
        [SerializeField, Range(2, 100)] private int m_cellCount = 10;

        [SerializeField] private List<Cell> m_cells = new();

        public List<Cell> GetCells()
        {
            return m_cells;
        }

        public override void OnDrawGizmos()
        {
            if (m_cells.Count == 0)
            {
                return;
            }

            Gizmos.color = new Color(1, 1, 1, 0.5f);

            foreach (var cell in m_cells)
            {
                Gizmos.DrawWireCube(cell.Coords.CellToWorldSpace(), new Vector3(8f, 8f, 8f));
            }
        }

        protected override bool OnGenerate()
        {
            m_cells = new();

            var startCell = new Cell
            {
                Coords = new Vector2Int(0, 0)
            };
            m_cells.Add(startCell);

            for (int i = 1; i < m_cellCount; i++)
            {
                var cellIndex = i - 1;
                Cell newCell = null;
                while (newCell == null)
                {
                    newCell = GetNewCell(m_cells[cellIndex]);
                    cellIndex--;
                }

                m_cells.Add(newCell);
            }

            return true;
        }

        private Cell GetNewCell(Cell lastCell)
        {
            var availableDirections = GetAvailableDirections(lastCell.Coords);
            if (availableDirections.Count == 0)
            {
                return null;
            }

            var randomDirection = availableDirections[UnityEngine.Random.Range(0, availableDirections.Count)];

            var newCell = new Cell
            {
                Coords = lastCell.Coords + randomDirection.GetVector()
            };

            return newCell;
        }

        private List<RoomEntranceDirection> GetAvailableDirections(Vector2Int currentCoords)
        {
            var availableDirections = new List<RoomEntranceDirection>();

            foreach (var direction in (RoomEntranceDirection[])Enum.GetValues(typeof(RoomEntranceDirection)))
            {
                var newCoords = currentCoords + direction.GetVector();

                if (m_cells.Any(c => c.Coords == newCoords))
                {
                    continue;
                }

                availableDirections.Add(direction);
            }

            return availableDirections;
        }
    }
}