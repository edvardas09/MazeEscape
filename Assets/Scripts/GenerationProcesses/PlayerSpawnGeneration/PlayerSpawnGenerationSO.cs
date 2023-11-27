using MazeEscape.MazeGenerator;
using UnityEngine;

namespace MazeEscape.GenerationProcesses
{
    [CreateAssetMenu(fileName = "PlayerSpawnGeneration", menuName = "Maze/Generation/PlayerSpawnGeneration")]
    public class PlayerSpawnGenerationSO : GenerationProcessSO
    {
        [SerializeField] private GameObject m_playerPrefab;

        private GameObject m_player;

        protected override bool OnGenerate()
        {
            if (m_player)
            {
                DestroyImmediate(m_player);
            }
            
            if (!TryGetProcess<IStartCell>(out var startProcess))
            {
                Debug.LogError("PlayerSpawnGenerationSO requires a IStartCell to be present in the MazeGeneratorSO");
                return false;
            }

            var startCell = startProcess.GetStartCell();
            m_player = Instantiate(m_playerPrefab, startCell.Coords.CellToWorldSpace(1), Quaternion.identity);

            return true;
        }
    }
}