using MazeEscape.Gameplay.State;
using MazeEscape.MazeGenerator;
using System.Collections.Generic;
using UnityEngine;
using MazeEscape.Extensions;
using Zenject;

namespace MazeEscape.GenerationProcesses
{   
    [CreateAssetMenu(fileName = "EnemyGeneration", menuName = "Maze/Generation/EnemyGeneration")]
    public class EnemyGenerationSO : GenerationProcessSO
    {
        [SerializeField] private GameObject m_enemyPrefab;
        [SerializeField, Range(0, 10)] private int m_enemyCount = 2;

        [SerializeField] private List<GameObject> m_enemies;

        private IStartCell m_startCellGeneration;
        private ICellGeneration m_cellGeneration;

        [Inject]
        public void Initialize(IStartCell startCellGeneration, ICellGeneration cellGeneration)
        {
            m_startCellGeneration = startCellGeneration;
            m_cellGeneration = cellGeneration;
        }

        public override bool IsCompleted()
        {
            // Always regenerate visual assets
            return false;
        }

        public override void Reset()
        {
            foreach (var enemy in m_enemies)
            {
                DestroyImmediate(enemy);
            }

            m_enemies = new List<GameObject>();
        }

        protected override bool OnGenerate()
        {
            if(!m_mazeGenerator.TryGetProcess<NavMeshGenerationSO>(out _))
            {
                Debug.LogError("Could not find NavMeshGenerationSO");
                return false;
            }

            foreach (var enemy in m_enemies)
            {
                DestroyImmediate(enemy);
            }

            var startCells = m_startCellGeneration.GetStartCells();
            var cells = m_cellGeneration.GetCells();
            var availableCells = new List<Cell>(cells);
            availableCells.RemoveAll(x => startCells.Contains(x));

            for (var i = 0; i < m_enemyCount; i++)
            {
                var randomCell = availableCells[Random.Range(0, availableCells.Count)];

                var enemy = Instantiate(m_enemyPrefab, randomCell.Coords.CellToWorldSpace(1), Quaternion.identity);
                enemy.transform.parent = m_mazeGenerator.transform;

                m_enemies.Add(enemy);

                if (!enemy.TryGetComponent<StateMachine>(out var stateMachine))
                {
                    Debug.LogError("Enemy prefab does not have a StateMachine component");
                    return false;
                }

                availableCells.Remove(randomCell);
                stateMachine.Initialize(m_mazeGenerator);

                if (availableCells.Count < 1)
                {
                    availableCells.AddRange(cells);
                    availableCells.RemoveAll(x => startCells.Contains(x));
                }
            }

            return true;
        }
    }
}