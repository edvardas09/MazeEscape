using MazeEscape.MazeGenerator;
using UnityEngine;
using MazeEscape.Extensions;
using Zenject;
using System.Collections.Generic;

namespace MazeEscape.GenerationProcesses
{
    [CreateAssetMenu(fileName = "StartGeneration", menuName = "Maze/Generation/StartGeneration")]
    public class StartGenerationSO : GenerationProcessSO, IStartCell
    {
        [SerializeField] private List<Cell> m_startCells = new();

        private ICellGeneration m_cellGeneration;

        [Inject]
        public void Initialize(ICellGeneration cellGeneration)
        {
            m_cellGeneration = cellGeneration;
        }

        public override bool IsCompleted()
        {
            return m_startCells.Count > 0;
        }

        public override void Reset()
        {
            m_startCells.Clear();
        }

        public List<Cell> GetStartCells()
        {
            return m_startCells;
        }

        public override void OnDrawGizmos()
        {
            if (m_startCells == null)
            {
                return;
            }

            Gizmos.color = new Color(0, 1, 0, 0.5f);
            foreach (var cell in m_startCells)
            {
                Gizmos.DrawWireCube(cell.Coords.CellToWorldSpace(), new Vector3(10, 10, 10));
            }
        }

        protected override bool OnGenerate()
        {
            var cells = m_cellGeneration.GetCells();
            m_startCells.Add(cells[Random.Range(0, cells.Count)]);
            return true;
        }
    }
}