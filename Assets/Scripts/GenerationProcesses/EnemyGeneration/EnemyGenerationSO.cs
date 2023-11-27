using MazeEscape.Gameplay.State;
using MazeEscape.MazeGenerator;
using UnityEngine;

namespace MazeEscape.GenerationProcesses
{   
    [CreateAssetMenu(fileName = "EnemyGeneration", menuName = "Maze/Generation/EnemyGeneration")]
    public class EnemyGenerationSO : GenerationProcessSO
    {
        [SerializeField] private GameObject enemyPrefab;

        private GameObject enemy;

        protected override bool OnGenerate()
        {
            if (enemy)
            {
                DestroyImmediate(enemy);
            }

            if (!TryGetProcess<IFinishCell>(out var finishCellProcess))
            {
                Debug.LogError("Finish cell not found");
                return false;
            }

            var finishCell = finishCellProcess.GetFinishCell();
            enemy = Instantiate(enemyPrefab, finishCell.Coords.CellToWorldSpace(1), Quaternion.identity);
            if (!enemy.TryGetComponent<StateMachine>(out var stateMachine))
            {
                Debug.LogError("Enemy prefab does not have a StateMachine component");
                return false;
            }

            stateMachine.Initialize(m_mazeGenerator);

            return true;
        }
    }
}