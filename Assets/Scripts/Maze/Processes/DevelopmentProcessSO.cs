using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MazeEscape.Maze
{
    [CreateAssetMenu(fileName = "DevelopmentProcess", menuName = "Maze/Generation/DevelopmentProcess")]
    public class DevelopmentProcessSO : GenerationProcessSO
    {
        [SerializeField] private List<Cell> remainingCells = new List<Cell>();
        [SerializeField] private List<Cell> currentIterationCells = new List<Cell>();

        private List<Passage> passages = new List<Passage>();

        public override void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            foreach (var cell in currentIterationCells)
            {
                Gizmos.DrawWireCube(new Vector3(cell.Coords.x, 0, cell.Coords.y), new Vector3(.5f, .5f, .5f));
            }
        }

        protected override void OnGenerate()
        {
            currentIterationCells.Clear();
            remainingCells.Clear();

            var cellGeneration = GetProcess<ICellGeneration>();
            var cells = cellGeneration.GetCells();
            remainingCells.AddRange(cells);

            var passageGeneration = GetProcess<IEntranceGeneration>();
            passages = passageGeneration.GetPassages();

            var startCell = GetProcess<IStartCell>().GetStartCell();
            currentIterationCells.Add(startCell);

            var iterationCount = cells.Count;
            while (iterationCount > 0)
            {
                iterationCount--;
                var tempCells = Iterate();
                if (tempCells.Count == 0)
                {
                    break;
                }

                Debug.Log(tempCells.Count);
                currentIterationCells = tempCells;
            }
        }

        [ContextMenu("TestIterate")]
        public void TestIterate()
        {
            currentIterationCells = Iterate();
        }

        public List<Cell> Iterate()
        {
            var newCells = new List<Cell>();

            for (int i = 0; i < currentIterationCells.Count; i++)
            {
                Cell cell = currentIterationCells[i];
                remainingCells.Remove(cell);
                var availablePassages = passages.Where(p => p.EntranceCell == cell).ToList();
                if (availablePassages.Count == 0)
                {
                    continue;
                }

                availablePassages = availablePassages.Where(x => remainingCells.Contains(x.ExitCell)).ToList();
                availablePassages = availablePassages.Where(x => !newCells.Contains(x.ExitCell)).ToList();

                newCells.AddRange(availablePassages.Select(x => x.ExitCell).ToList());
            }

            return newCells;
        }
    }
}
