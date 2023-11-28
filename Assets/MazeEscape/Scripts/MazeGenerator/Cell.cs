using System;
using UnityEngine;

namespace MazeEscape.MazeGenerator
{
    [Serializable]
    public class Cell
    {
        [SerializeField] public Vector2Int Coords;
    }
}