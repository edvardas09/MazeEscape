using MazeEscape.Gameplay.State;
using MazeEscape.MazeGenerator;
using UnityEngine;

namespace MazeEscape.GenerationProcesses
{   
    [CreateAssetMenu(fileName = "EnemyGeneration", menuName = "Maze/Generation/EnemyGeneration")]
    public class EnemyGenerationSO : GenerationProcessSO
    {
        [SerializeField] private GameObject m_enemyPrefab;

        private GameObject m_enemy;

        protected override bool OnGenerate()
        {
            if (m_enemy)
            {
                DestroyImmediate(m_enemy);
            }

            if (!TryGetProcess<IFinishCell>(out var finishCellProcess))
            {
                Debug.LogError("Finish cell not found");
                return false;
            }

            var finishCell = finishCellProcess.GetFinishCell();
            m_enemy = Instantiate(m_enemyPrefab, finishCell.Coords.CellToWorldSpace(1), Quaternion.identity);
            if (!m_enemy.TryGetComponent<StateMachine>(out var stateMachine))
            {
                Debug.LogError("Enemy prefab does not have a StateMachine component");
                return false;
            }

            stateMachine.Initialize(m_mazeGenerator);

            return true;
        }
    }
}