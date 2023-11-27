using MazeEscape.MazeGenerator;
using UnityEngine;

namespace MazeEscape.GenerationProcesses
{
    [CreateAssetMenu(fileName = "FinishVisualGeneration", menuName = "Maze/Generation/FinishVisualGeneration")]
    public class FinishVisualGenerationSO : GenerationProcessSO
    {
        [SerializeField] private GameObject m_finishPrefab;

        protected override bool OnGenerate()
        {
            if (!TryGetProcess<IFinishCell>(out var finishProcess))
            {
                Debug.LogError("FinishVisualGenerationSO requires a IFinishCell to be present in the MazeGeneratorSO");
                return false;
            }

            var finishCell = finishProcess.GetFinishCell();
            var finishCellGameObject = Instantiate(m_finishPrefab, finishCell.Coords.CellToWorldSpace(), Quaternion.identity);
            finishCellGameObject.transform.parent = m_mazeGenerator.transform;
            return true;
        }
    }
}