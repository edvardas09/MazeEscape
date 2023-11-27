using MazeEscape.MazeGenerator;
using UnityEngine;

namespace MazeEscape.GenerationProcesses
{
    [CreateAssetMenu(fileName = "StartVisualGeneration", menuName = "Maze/Generation/StartVisualGeneration")]
    public class StartVisualGenerationSO : GenerationProcessSO
    {
        [SerializeField] private GameObject m_startPrefab;

        protected override bool OnGenerate()
        {
            if (!TryGetProcess<IStartCell>(out var startProcess))
            {
                Debug.LogError("StartVisualGenerationSO requires a IStartCell to be present in the MazeGeneratorSO");
                return false;
            }

            var startCell = startProcess.GetStartCell();
            var startCellGameObject = Instantiate(m_startPrefab, startCell.Coords.CellToWorldSpace(), Quaternion.identity);
            startCellGameObject.transform.parent = m_mazeGenerator.transform;

            return true;
        }
    }
}