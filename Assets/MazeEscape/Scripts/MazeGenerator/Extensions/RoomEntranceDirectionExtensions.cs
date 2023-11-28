using MazeEscape.MazeGenerator.Room;
using UnityEngine;

namespace MazeEscape.MazeGenerator
{
    public static class RoomEntranceDirectionExtensions
    {
        public static RoomEntranceDirection GetOpposite(this RoomEntranceDirection direction)
        {
            return direction switch
            {
                RoomEntranceDirection.Top => RoomEntranceDirection.Bottom,
                RoomEntranceDirection.Right => RoomEntranceDirection.Left,
                RoomEntranceDirection.Bottom => RoomEntranceDirection.Top,
                RoomEntranceDirection.Left => RoomEntranceDirection.Right,
                _ => default,
            };
        }

        public static Vector2Int GetVector(this RoomEntranceDirection direction)
        {
            return direction switch
            {
                RoomEntranceDirection.Top => Vector2Int.up,
                RoomEntranceDirection.Right => Vector2Int.right,
                RoomEntranceDirection.Bottom => Vector2Int.down,
                RoomEntranceDirection.Left => Vector2Int.left,
                _ => default,
            };
        }
    }
}