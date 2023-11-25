using UnityEngine;

namespace MazeEscape.Maze
{
    [CreateAssetMenu(fileName = "StartGeneration", menuName = "Maze/Generation/StartGeneration")]
    public class StartGenerationSO : GenerationProcessSO, IStartCell
    {
        [SerializeField] private Cell m_startCell;

        public Cell GetStartCell()
        {
            return m_startCell;
        }

        public override void OnDrawGizmos()
        {
            if (m_startCell == null)
            {
                return;
            }

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(new Vector3(m_startCell.Coords.x, 0, m_startCell.Coords.y), new Vector3(1, 1, 1));
        }

        protected override void OnGenerate()
        {
            var cellGeneration = GetProcess<ICellGeneration>();
            var cells = cellGeneration.GetCells();

            m_startCell = cells[Random.Range(0, cells.Count)];
        }
    }
}