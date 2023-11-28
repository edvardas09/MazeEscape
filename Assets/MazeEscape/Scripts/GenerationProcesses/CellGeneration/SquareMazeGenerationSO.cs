using MazeEscape.MazeGenerator;
using System;
using System.Collections.Generic;
using UnityEngine;
using MazeEscape.Extensions;

namespace MazeEscape.GenerationProcesses
{
    [CreateAssetMenu(fileName = "SquareMazeGeneration", menuName = "Maze/Generation/SquareMazeGeneration")]
    public class SquareMazeGenerationSO : GenerationProcessSO, ICellGeneration
    {
        [SerializeField, Range(2, 10)] private int m_width = 10;
        [SerializeField, Range(2, 10)] private int m_height = 10;

        [SerializeField] private List<Cell> m_cells = new();
        
        public override bool IsCompleted()
        {
            return m_cells.Count > 0;
        }

        public override void Reset()
        {
            m_cells = new();
        }

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

            for (int x = 0; x < m_width; x++)
            {
                for (int y = 0; y < m_height; y++)
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