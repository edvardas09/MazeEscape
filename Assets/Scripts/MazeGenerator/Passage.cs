using MazeEscape.MazeGenerator.Room;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MazeEscape.MazeGenerator
{
    [Serializable]
    public class Passage
    {
        [SerializeField] public List<Cell> Cells;

        public RoomEntranceDirection GetDirection(Cell startCell)
        {
            if (Cells.Count < 2)
            {
                throw new Exception("Invalid passage");
            }
            
            var entranceCell = Cells.Find(cell => cell == startCell) ?? throw new Exception("Invalid entrance cell");
            var exitCell = Cells.Find(cell => cell != startCell);

            var direction = exitCell.Coords - entranceCell.Coords;

            // TODO: change to if else
            return direction switch
            {
                var d when d == Vector2Int.up => RoomEntranceDirection.Top,
                var d when d == Vector2Int.right => RoomEntranceDirection.Right,
                var d when d == Vector2Int.down => RoomEntranceDirection.Bottom,
                var d when d == Vector2Int.left => RoomEntranceDirection.Left,
                _ => throw new Exception("Invalid direction"),
            };
        }
    }
}