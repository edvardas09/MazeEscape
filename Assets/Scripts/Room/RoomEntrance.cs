using MazeEscape.Room;
using UnityEngine;

public class RoomEntrance : MonoBehaviour
{
    [SerializeField] private RoomEntranceDirection roomEntranceDirection;

    public RoomEntranceDirection RoomEntranceDirection => roomEntranceDirection;

    public bool IsEntranceBetweenRooms(RoomEntranceDirection roomEntranceDirection)
    {
        return roomEntranceDirection switch
        {
            RoomEntranceDirection.Top => this.roomEntranceDirection == RoomEntranceDirection.Bottom,
            RoomEntranceDirection.Right => this.roomEntranceDirection == RoomEntranceDirection.Left,
            RoomEntranceDirection.Bottom => this.roomEntranceDirection == RoomEntranceDirection.Top,
            RoomEntranceDirection.Left => this.roomEntranceDirection == RoomEntranceDirection.Right,
            _ => false,
        };
    }
}
