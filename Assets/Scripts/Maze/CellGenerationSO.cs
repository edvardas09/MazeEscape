using MazeEscape.Room;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MazeEscape.Maze
{
    [CreateAssetMenu(fileName = "CellGeneration", menuName = "Maze/Generation/CellGeneration")]
    public class CellGeneration : GenerationProcessSO, ICellGeneration
    {
        [SerializeField] private int m_cellCount = 10;

        [SerializeField] private List<Cell> m_cells = new();

        public override void OnDrawGizmos()
        {
            if (m_cells.Count == 0)
            {
                return;
            }

            foreach (var cell in m_cells)
            {
                Gizmos.DrawWireCube(new Vector3(cell.Coords.x, 0, cell.Coords.y), new Vector3(1, 1, 1));
            }
        }

        protected override void OnGenerate()
        {
            m_cells = new();

            var startCell = new Cell
            {
                Coords = new Vector2Int(0, 0)
            };
            m_cells.Add(startCell);

            for (int i = 1; i < m_cellCount; i++)
            {
                var newCell = GetNewCell();

                //TODO: continue generation from different cell
                if (newCell == null)
                {
                    break;
                }

                m_cells.Add(newCell);
            }
        }

        private Cell GetNewCell()
        {
            var availableDirections = GetAvailableDirections(m_cells.Last().Coords);
            if (availableDirections.Count == 0)
            {
                return null;
            }

            var randomDirection = availableDirections[UnityEngine.Random.Range(0, availableDirections.Count)];

            var newCell = new Cell
            {
                Coords = m_cells.Last().Coords + randomDirection.GetVector()
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

        public List<Cell> GetCells()
        {
            return m_cells;
        }
    }
}