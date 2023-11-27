using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// this script is for the development & testing of the maze generation process
namespace MazeEscape.MazeGenerator
{
    [CreateAssetMenu(fileName = "DevelopmentProcess", menuName = "Maze/Generation/DevelopmentProcess")]
    public class DevelopmentProcessSO : GenerationProcessSO
    {
        [SerializeField] private List<Cell> remainingCells = new();
        [SerializeField] private List<Cell> currentIterationCells = new();

        private List<Passage> passages = new();

        public override void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;

            foreach (var cell in currentIterationCells)
            {
                Gizmos.DrawWireCube(new Vector3(cell.Coords.x, 0, cell.Coords.y), new Vector3(.5f, .5f, .5f));
            }
        }

        protected override bool OnGenerate()
        {
            currentIterationCells.Clear();
            remainingCells.Clear();

            if (!TryGetProcess<ICellGeneration>(out var cellGeneration))
            {
                Debug.LogError("DevelopmentProcessSO requires a ICellGeneration to be present in the MazeGeneratorSO");
                return false;
            }

            if (!TryGetProcess<IEntranceGeneration>(out var passageGeneration))
            {
                Debug.LogError("DevelopmentProcessSO requires a IEntranceGeneration to be present in the MazeGeneratorSO");
                return false;
            }
            
            if (!TryGetProcess<IStartCell>(out var startCellGeneration))
            {
                Debug.LogError("DevelopmentProcessSO requires a IStartCell to be present in the MazeGeneratorSO");
                return false;
            }

            var cells = cellGeneration.GetCells();
            remainingCells.AddRange(cells);

            passages = passageGeneration.GetPassages();

            var startCell = startCellGeneration.GetStartCell();
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

                currentIterationCells = tempCells;
            }

            return true;
        }

        [ContextMenu("TestIterate")]
        private void TestIterate()
        {
            currentIterationCells = Iterate();
        }

        private List<Cell> Iterate()
        {
            var newCells = new List<Cell>();

            for (int i = 0; i < currentIterationCells.Count; i++)
            {
                var cell = currentIterationCells[i];
                remainingCells.Remove(cell);
                var availablePassages = passages.Where(p => p.Cells.Contains(cell)).ToList();
                if (availablePassages.Count == 0)
                {
                    continue;
                }

                availablePassages = availablePassages.Where(x => remainingCells.Contains(x.Cells.First(x => x != cell))).ToList();
                availablePassages = availablePassages.Where(x => !newCells.Contains(x.Cells.First(x => x != cell))).ToList();

                newCells.AddRange(availablePassages.Select(x => x.Cells.First(x => x != cell)).ToList());
            }

            return newCells;
        }
    }
}