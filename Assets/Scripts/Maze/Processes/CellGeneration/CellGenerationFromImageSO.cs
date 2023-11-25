using System.Collections.Generic;
using UnityEngine;

namespace MazeEscape.Maze
{
    [CreateAssetMenu(fileName = "CellGenerationFromImage", menuName = "Maze/Generation/CellGenerationFromImage")]
    public class CellGenerationFromImageSO : GenerationProcessSO, ICellGeneration
    {
        [SerializeField] private Sprite sprite;
        [SerializeField] private List<Cell> m_cells = new();

        public override void OnDrawGizmos()
        {
            foreach (var cell in m_cells)
            {
                Gizmos.DrawWireCube(new Vector3(cell.Coords.x, 0, cell.Coords.y), new Vector3(1, 1, 1));
            }
        }

        public List<Cell> GetCells()
        {
            return m_cells;
        }

        protected override void OnGenerate()
        {
            m_cells = new();

            var texture = sprite.texture;
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
        }
    }
}