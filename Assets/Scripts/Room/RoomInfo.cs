using UnityEngine;

public class RoomInfo
{
    private RoomType roomType;
    private Vector2Int position;

    public RoomType RoomType => roomType;
    public Vector2Int Position => position;

    public RoomInfo() { }

    public RoomInfo(RoomType roomType, Vector2Int position)
    {
        this.roomType = roomType;
        this.position = position;
    }

    public void UpdatePosition(Vector2Int position)
    {
        this.position = position;
    }

    public void UpdateRoomType(RoomType roomType)
    {
        this.roomType = roomType;
    }
}
