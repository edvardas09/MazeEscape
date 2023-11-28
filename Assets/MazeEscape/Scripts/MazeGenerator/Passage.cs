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
            
            var entranceCell = Cells.Find(cell => cell.Coords == startCell.Coords) ?? throw new Exception("Invalid entrance cell");
            var exitCell = Cells.Find(cell => cell.Coords != startCell.Coords);

            var direction = exitCell.Coords - entranceCell.Coords;
            if (direction == Vector2.up)
                return RoomEntranceDirection.Top;
            else if (direction == Vector2.right)
                return RoomEntranceDirection.Right;
            else if (direction == Vector2.down)
                return RoomEntranceDirection.Bottom;
            else if (direction == Vector2.left)
                return RoomEntranceDirection.Left;
            else
            {
                throw new Exception("Invalid direction");
            }
        }
    }
}