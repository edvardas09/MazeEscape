using MazeEscape.Gameplay.State;
using MazeEscape.MazeGenerator;
using System.Collections.Generic;
using UnityEngine;

namespace MazeEscape.GenerationProcesses
{   
    [CreateAssetMenu(fileName = "EnemyGeneration", menuName = "Maze/Generation/EnemyGeneration")]
    public class EnemyGenerationSO : GenerationProcessSO
    {
        [SerializeField] private GameObject m_enemyPrefab;
        [SerializeField] private int m_enemyCount = 2;

        private GameObject m_enemy;

        protected override bool OnGenerate()
        {
            if (m_enemy)
            {
                DestroyImmediate(m_enemy);
            }

            if (!TryGetProcess<IStartCell>(out var startCellProcess))
            {
                Debug.LogError("Start cell not found");
                return false;
            }

            if (!TryGetProcess<ICellGeneration>(out var cellGenerationProcess))
            {
                Debug.LogError("ICellGeneration not found");
                return false;
            }

            var startCell = startCellProcess.GetStartCell();
            var cells = cellGenerationProcess.GetCells();
            var availableCells = new List<Cell>(cells);
            availableCells.Remove(startCell);

            for (var i = 0; i < m_enemyCount; i++)
            {
                var randomCell = availableCells[Random.Range(0, availableCells.Count)];
                m_enemy = Instantiate(m_enemyPrefab, randomCell.Coords.CellToWorldSpace(1), Quaternion.identity);
                if (!m_enemy.TryGetComponent<StateMachine>(out var stateMachine))
                {
                    Debug.LogError("Enemy prefab does not have a StateMachine component");
                    return false;
                }

                availableCells.Remove(randomCell);
                stateMachine.Initialize(m_mazeGenerator);

                if (availableCells.Count < 1)
                {
                    availableCells.AddRange(cells);
                    availableCells.Remove(startCell);
                }
            }

            return true;
        }
    }
}