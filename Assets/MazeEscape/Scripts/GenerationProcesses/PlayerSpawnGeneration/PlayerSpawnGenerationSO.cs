using MazeEscape.MazeGenerator;
using UnityEngine;
using MazeEscape.Extensions;
using Zenject;
using System.Linq;

namespace MazeEscape.GenerationProcesses
{
    [CreateAssetMenu(fileName = "PlayerSpawnGeneration", menuName = "Maze/Generation/PlayerSpawnGeneration")]
    public class PlayerSpawnGenerationSO : GenerationProcessSO
    {
        [SerializeField] private GameObject m_playerPrefab;

        [SerializeField] private GameObject m_player;

        private IStartCell m_startCellGeneration;

        [Inject]
        public void Initialize(IStartCell startCellGeneration)
        {
            m_startCellGeneration = startCellGeneration;
        }

        public override bool IsCompleted()
        {
            // Always regenerate visual assets
            return false;
        }

        public override void Reset()
        {
            if (m_player)
            {
                DestroyImmediate(m_player);
            }

            m_player = null;
        }

        protected override bool OnGenerate()
        {
            if (m_player)
            {
                DestroyImmediate(m_player);
            }

            var startCells = m_startCellGeneration.GetStartCells();
            m_player = Instantiate(m_playerPrefab, startCells.First().Coords.CellToWorldSpace(0.25f), Quaternion.identity);
            m_player.transform.parent = m_mazeGenerator.transform;

            return true;
        }
    }
}