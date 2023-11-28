using MazeEscape.MazeGenerator;
using System.Collections.Generic;
using UnityEngine;
using MazeEscape.Extensions;

namespace MazeEscape.GenerationProcesses
{
    [CreateAssetMenu(fileName = "CellGenerationFromImage", menuName = "Maze/Generation/CellGenerationFromImage")]
    public class ImageGenerationSO : GenerationProcessSO, ICellGeneration
    {
        [SerializeField] private Sprite m_sprite;
        [SerializeField] private List<Cell> m_cells = new();

        public override bool IsCompleted()
        {
            return m_cells.Count > 0;
        }

        public override void Reset()
        {
            m_cells = new();
        }

        public override void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 1, 1, 0.5f);

            foreach (var cell in m_cells)
            {
                Gizmos.DrawWireCube(cell.Coords.CellToWorldSpace(), new Vector3(10, 10, 10));
            }
        }

        public List<Cell> GetCells()
        {
            return m_cells;
        }

        protected override bool OnGenerate()
        {
            m_cells = new();

            var texture = m_sprite.texture;
            var pixels = texture.GetPixels();
            var width = texture.width;

            for (int i = 0; i < pixels.Length; i++)
            {
                var pixel = pixels[i];
                if (!(pixel.r > 0.9f && pixel.g > 0.9f && pixel.b > 0.9f))
                {
                    continue;
                }

                var x = i % width;
                var y = i / width;

                var cell = new Cell
                {
                    Coords = new Vector2Int(x, y)
                };
                m_cells.Add(cell);
            }

            return true;
        }
    }
}