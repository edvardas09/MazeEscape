using MazeEscape.MazeGenerator;
using UnityEngine;

namespace MazeEscape.GenerationProcesses
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

            Gizmos.color = new Color(0, 1, 0, 0.5f);
            Gizmos.DrawWireCube(m_startCell.Coords.CellToWorldSpace(), new Vector3(10, 10, 10));
        }

        protected override bool OnGenerate()
        {
            if (!TryGetProcess<ICellGeneration>(out var cellGeneration))
            {
                Debug.LogError("StartGenerationSO requires a ICellGeneration to be present in the MazeGeneratorSO");
                return false;
            }

            var cells = cellGeneration.GetCells();

            m_startCell = cells[Random.Range(0, cells.Count)];
            return true;
        }
    }
}