using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MazeEscape.Maze
{
    [CreateAssetMenu(fileName = "EntranceGeneration", menuName = "Maze/Generation/EntranceGeneration")]
    public class EntranceGenerationSO : GenerationProcessSO, IEntranceGeneration
    {
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

            foreach (var passage in m_passages)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(new Vector3(passage.EntranceCell.Coords.x, 0, passage.EntranceCell.Coords.y), new Vector3(passage.ExitCell.Coords.x, 0, passage.ExitCell.Coords.y));
            }
        }

        protected override void OnGenerate()
        {
            m_passages = new();

            var cellGeneration = GetProcess<ICellGeneration>();

            var cells = cellGeneration.GetCells();

            foreach (var cell in cells)
            {
                var topCell = GetCell(cells, cell.Coords + Vector2Int.up);
                if (topCell != null)
                {
                    m_passages.Add(new Passage
                    {
                        EntranceCell = cell,
                        ExitCell = topCell
                    });
                }

                var rightCell = GetCell(cells, cell.Coords + Vector2Int.right);
                if (rightCell != null)
                {
                    m_passages.Add(new Passage
                    {
                        EntranceCell = cell,
                        ExitCell = rightCell
                    });
                }

                var bottomCell = GetCell(cells, cell.Coords + Vector2Int.down);
                if (bottomCell != null)
                {
                    m_passages.Add(new Passage
                    {
                        EntranceCell = cell,
                        ExitCell = bottomCell
                    });
                }

                var leftCell = GetCell(cells, cell.Coords + Vector2Int.left);
                if (leftCell != null)
                {
                    m_passages.Add(new Passage
                    {
                        EntranceCell = cell,
                        ExitCell = leftCell
                    });
                }


            }
        }

        private Cell GetCell(List<Cell> cells, Vector2Int coords)
        {
            return cells.FirstOrDefault(x => x.Coords == coords);
        }
    }
}