using MazeEscape.MazeGenerator;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MazeEscape.GenerationProcesses
{
    [CreateAssetMenu(fileName = "SquareMazeGeneration", menuName = "Maze/Generation/SquareMazeGeneration")]
    public class SquareMazeGeneration : GenerationProcessSO, ICellGeneration
    {
        [SerializeField, Range(2, 10)] private int width = 10;
        [SerializeField, Range(2, 10)] private int height = 10;

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

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var cell = new Cell
                    {
                        Coords = new Vector2Int(x, y)
                    };

                    m_cells.Add(cell);
                }
            }

            return true;
        }
    }
}