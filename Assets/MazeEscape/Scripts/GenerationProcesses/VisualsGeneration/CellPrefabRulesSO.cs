using System;
using System.Collections.Generic;
using UnityEngine;

namespace MazeEscape.GenerationProcesses
{
    [CreateAssetMenu(fileName = "CellPrefabRules", menuName = "Maze/CellPrefabRules", order = 1)]
    public class CellPrefabRulesSO : ScriptableObject
    {
        [Serializable]
        public struct CellPrefabRule
        {
            public RuleDirections Directions;
            public List<GameObject> Prefabs;
        }

        public List<CellPrefabRule> Rules;
    }
}