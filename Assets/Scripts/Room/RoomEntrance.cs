using UnityEngine;

public class RoomEntrance : MonoBehaviour
{
    [SerializeField] private RoomEntranceDirection roomEntranceDirection;

    public RoomEntranceDirection RoomEntranceDirection => roomEntranceDirection;

    private void OnValidate()
    {
        if (roomEntranceDirection == RoomEntranceDirection.None)
        {
            Debug.LogError("RoomEntranceDirection is not set", this);
        }
    }

    public bool IsEntranceBetweenRooms(RoomEntranceDirection roomEntranceDirection)
    {
        switch (roomEntranceDirection)
        {
            case RoomEntranceDirection.Top:
                return this.roomEntranceDirection == RoomEntranceDirection.Bottom;
            case RoomEntranceDirection.Right:
                return this.roomEntranceDirection == RoomEntranceDirection.Left;
            case RoomEntranceDirection.Bottom:
                return this.roomEntranceDirection == RoomEntranceDirection.Top;
            case RoomEntranceDirection.Left:
                return this.roomEntranceDirection == RoomEntranceDirection.Right;
            default:
                return false;
        }
    }
}
