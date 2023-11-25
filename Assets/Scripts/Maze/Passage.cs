using MazeEscape.Room;
using System;
using UnityEngine;

namespace MazeEscape.Maze
{
    [Serializable]
    public class Passage
    {
        [SerializeField] public Cell EntranceCell;
        [SerializeField] public Cell ExitCell;

        public RoomEntranceDirection GetDirection()
        {
            var direction = ExitCell.Coords - EntranceCell.Coords;
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