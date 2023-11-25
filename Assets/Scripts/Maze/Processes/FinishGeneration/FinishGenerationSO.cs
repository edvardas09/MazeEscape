using UnityEngine;

namespace MazeEscape.Maze
{
    [CreateAssetMenu(fileName = "FinishGeneration", menuName = "Maze/Generation/FinishGeneration")]
    public class FinishGenerationSO : GenerationProcessSO, IFinishCell
    {
        [SerializeField] private Cell m_finishCell;

        public Cell GetFinishCell()
        {
            return m_finishCell;
        }

        public override void OnDrawGizmos()
        {
            if (m_finishCell == null)
            {
                return;
            }

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(new Vector3(m_finishCell.Coords.x, 0, m_finishCell.Coords.y), new Vector3(1, 1, 1));
        }

        protected override void OnGenerate()
        {
            var cellGeneration = GetProcess<ICellGeneration>();
            var cells = cellGeneration.GetCells();

            var startGeneration = GetProcess<IStartCell>();
            var startCell = startGeneration.GetStartCell();

            var furthestCell = startCell;
            var furthestDistance = 0f;
            foreach (var cell in cells)
            {
                var distance = Vector2Int.Distance(startCell.Coords, cell.Coords);
                if (distance > furthestDistance)
                {
                    furthestCell = cell;
                    furthestDistance = distance;
                }
            }

            m_finishCell = furthestCell;
        }
    }
}