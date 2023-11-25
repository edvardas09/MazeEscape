using MazeEscape.Room;
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
                AddEntranceToCoords(cells, cell, RoomEntranceDirection.Top);
                AddEntranceToCoords(cells, cell, RoomEntranceDirection.Right);
                AddEntranceToCoords(cells, cell, RoomEntranceDirection.Bottom);
                AddEntranceToCoords(cells, cell, RoomEntranceDirection.Left);
            }
        }

        private void AddEntranceToCoords(List<Cell> cells, Cell cell, RoomEntranceDirection roomEntranceDirection)
        {
            var neighbourCell = GetCell(cells, cell.Coords + roomEntranceDirection.GetVector());
            if (neighbourCell != null)
            {
                m_passages.Add(new Passage
                {
                    EntranceCell = cell,
                    ExitCell = neighbourCell
                });
            }
        }

        private Cell GetCell(List<Cell> cells, Vector2Int coords)
        {
            return cells.FirstOrDefault(x => x.Coords == coords);
        }
    }
}