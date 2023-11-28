using System.Collections.Generic;
using MazeEscape.MazeGenerator;
using MazeEscape.MazeGenerator.Room;
using UnityEngine;
using MazeEscape.Extensions;
using Zenject;
using System.Linq;

namespace MazeEscape.GenerationProcesses
{
    [CreateAssetMenu(fileName = "VisualsGeneration", menuName = "Maze/Generation/VisualsGeneration")]
    public class VisualsGenerationSO : GenerationProcessSO
    {
        [SerializeField] private CellPrefabRulesSO m_cellPrefabRules;

        [SerializeField] private List<GameObject> m_generatedObjects;

        private ICellGeneration m_cellGeneration;
        private IEntranceGeneration m_entranceGeneration;

        [Inject]
        public void Initialize(ICellGeneration cellGeneration, IEntranceGeneration entranceGeneration)
        {
            m_cellGeneration = cellGeneration;
            m_entranceGeneration = entranceGeneration;
        }
        
        public override bool IsCompleted()
        {
            var listHasNull = m_generatedObjects.FindAll(o => o == null).Count > 0;
            return m_generatedObjects.Count > 0 && !listHasNull;
        }

        public override void Reset()
        {
            foreach (var generatedObject in m_generatedObjects)
            {
                DestroyImmediate(generatedObject);
            }

            m_generatedObjects = new List<GameObject>();
        }

        protected override bool OnGenerate()
        {
            var cells = m_cellGeneration.GetCells();
            var passages = m_entranceGeneration.GetPassages();

            var mazeTransform = m_mazeGenerator.transform;
            foreach (var cell in cells)
            {
                if(!TryGenerateCell(cell, passages, out var gameObject))
                {
                    continue;
                }

                gameObject.transform.SetParent(mazeTransform);
                m_generatedObjects.Add(gameObject);
            }

            return true;
        }

        private bool TryGenerateCell(Cell cell, List<Passage> passages, out GameObject gameObject)
        {
            gameObject = null;

            var cellPassages = passages.Where(p => p.Cells.Any(x => x.Coords == cell.Coords)).ToList();
            var directionsFlags = GetDirectionsFlags(cell, cellPassages);
            if (!TryGetPrefabByDirectionsFlags(directionsFlags, out var selectedPrefab))
            {
                Debug.LogError($"Failed to generate cell for coords ({cell.Coords})");
                return false;
            }

            gameObject = Instantiate(selectedPrefab, cell.Coords.CellToWorldSpace(), Quaternion.identity);
            return true;
        }

        private bool TryGetPrefabByDirectionsFlags(RuleDirections directionsFlags, out GameObject prefab)
        {
            prefab = null;

            var availableRules = m_cellPrefabRules.Rules.FindAll(r => r.Directions.Compare(directionsFlags));
            if (availableRules.Count <= 0)
            {
                Debug.LogError($"No prefab rules found for directions {directionsFlags}");
                return false;
            }

            var prefabs = availableRules[Random.Range(0, availableRules.Count)].Prefabs;
            if(prefabs.Count <= 0)
            {
                Debug.LogError($"No prefabs found for directions {directionsFlags}");
                return false;
            }

            prefab = prefabs[Random.Range(0, prefabs.Count)];
            return true;
        }

        private RuleDirections GetDirectionsFlags(Cell cell, List<Passage> cellPassages)
        {
            var directionsFlags = RuleDirections.None;
            foreach (var passage in cellPassages)
            {
                switch (passage.GetDirection(cell))
                {
                    case RoomEntranceDirection.Top:
                        directionsFlags |= RuleDirections.North;
                        break;
                    case RoomEntranceDirection.Right:
                        directionsFlags |= RuleDirections.East;
                        break;
                    case RoomEntranceDirection.Bottom:
                        directionsFlags |= RuleDirections.South;
                        break;
                    case RoomEntranceDirection.Left:
                        directionsFlags |= RuleDirections.West;
                        break;
                }
            }

            return directionsFlags;
        }
    }
}