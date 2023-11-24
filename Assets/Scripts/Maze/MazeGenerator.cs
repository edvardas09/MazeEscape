using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private List<Room> roomPrefabs;
    [SerializeField] private int mazeLength;

    private List<Room> spawnedRooms = new();

    private void Awake()
    {
        GenerateMaze();
    }

    private void GenerateMaze()
    {
        var randomStartRoom = roomPrefabs[Random.Range(0, roomPrefabs.Count)];
        var startRoom = Instantiate(randomStartRoom, transform);
        startRoom.Setup(new RoomInfo(RoomType.Start, new Vector2Int(0,0)));
        spawnedRooms.Add(startRoom);

        var previousRoom = startRoom;

        for (int i = 0; i < mazeLength; i++)
        {
            var randomRoom = GetRandomAvailableRoom(previousRoom, false);

            var room = Instantiate(randomRoom, transform);

            Debug.Log($"randomRoom.RoomInfo.Position {randomRoom.RoomInfo.Position}");

            room.transform.localPosition = new Vector3(15 * randomRoom.RoomInfo.Position.y, 0, 15 * randomRoom.RoomInfo.Position.x);
            Debug.Log($"room.transform.position {room.transform.position}");

            room.Setup(randomRoom.RoomInfo);
            spawnedRooms.Add(room);

            previousRoom = room;
        }

        var randomEndRoom = GetRandomAvailableRoom(previousRoom, true);
        randomEndRoom.RoomInfo.UpdateRoomType(RoomType.End);

        var endRoom = Instantiate(randomEndRoom, transform);
        endRoom.transform.localPosition = new Vector3(15 * randomEndRoom.RoomInfo.Position.y, 0, 15 * randomEndRoom.RoomInfo.Position.x);

        endRoom.Setup(randomEndRoom.RoomInfo);
        spawnedRooms.Add(endRoom);
    }

    private Room GetRandomAvailableRoom(Room previousRoom, bool includeDeadEnds)
    {
        var availableRooms = new List<Room>(roomPrefabs);

        //Remove dead ends if needed
        if (!includeDeadEnds)
        {
            availableRooms = roomPrefabs.Where(room => room.RoomEntrances.Count > 1).ToList();
        }

        //Remove rooms that don't have an entrance from the previous room
        availableRooms = availableRooms.Where(
                        room => room.RoomEntrances.Any(roomEntrance => previousRoom.RoomEntrances.Any(
                            previousRoomEntrance => previousRoomEntrance.IsEntranceBetweenRooms(roomEntrance.RoomEntranceDirection)))).ToList();


        //Remove rooms that would be placed in occupied position
        var availableRoomsTempList = new List<Room>(availableRooms);

        foreach (var room in availableRoomsTempList)
        {
            var isRoomPositionOccupied = false;
            foreach (var roomEntrance in room.RoomEntrances)
            {
                var newPosition = previousRoom.RoomInfo.Position;

                switch (roomEntrance.RoomEntranceDirection)
                {
                    case RoomEntranceDirection.Top:
                        newPosition = new Vector2Int(newPosition.x, newPosition.y + 1);
                        isRoomPositionOccupied = IsRoomPositionOccupied(newPosition);
                        break;
                    case RoomEntranceDirection.Right:
                        newPosition = new Vector2Int(newPosition.x + 1, newPosition.y);
                        isRoomPositionOccupied = IsRoomPositionOccupied(newPosition);
                        break;
                    case RoomEntranceDirection.Bottom:
                        newPosition = new Vector2Int(newPosition.x, newPosition.y - 1);
                        isRoomPositionOccupied = IsRoomPositionOccupied(newPosition);
                        break;
                    case RoomEntranceDirection.Left:
                        newPosition = new Vector2Int(newPosition.x - 1, newPosition.y);
                        isRoomPositionOccupied = IsRoomPositionOccupied(newPosition);
                        break;
                    default:
                        break;
                }

                if (!isRoomPositionOccupied)
                {
                    room.RoomInfo.UpdatePosition(newPosition);
                    break;
                }
            }

            if (isRoomPositionOccupied)
            {
                availableRooms.Remove(room);
            }
        }


        //Remove rooms that doesn't fit
        var availableRoomsTempList2 = new List<Room>(availableRooms);
        foreach (var room in availableRoomsTempList2)
        {
            var canBePlacedTop = CanBePlacedNearby(room, RoomEntranceDirection.Top, new Vector2Int(room.RoomInfo.Position.x, room.RoomInfo.Position.y + 1), RoomEntranceDirection.Bottom);
            var canBePlacedRight = CanBePlacedNearby(room, RoomEntranceDirection.Right, new Vector2Int(room.RoomInfo.Position.x + 1, room.RoomInfo.Position.y), RoomEntranceDirection.Left);
            var canBePlacedBottom = CanBePlacedNearby(room, RoomEntranceDirection.Bottom, new Vector2Int(room.RoomInfo.Position.x, room.RoomInfo.Position.y - 1), RoomEntranceDirection.Top);
            var canBePlacedLeft = CanBePlacedNearby(room, RoomEntranceDirection.Left, new Vector2Int(room.RoomInfo.Position.x - 1, room.RoomInfo.Position.y), RoomEntranceDirection.Right);

            if (!canBePlacedTop || !canBePlacedRight || !canBePlacedBottom || !canBePlacedLeft)
            {
                availableRooms.Remove(room);
            }
        }

        return availableRooms[Random.Range(0, availableRooms.Count)];
    }

    private bool CanBePlacedNearby(Room room, RoomEntranceDirection roomEntranceDirection, Vector2Int neighboorPosition, RoomEntranceDirection neighboorEntranceDirection)
    {
        var canBePlaced = true;

        var nearbyRoom = spawnedRooms.FirstOrDefault(x => x.RoomInfo.Position == neighboorPosition);
        if (nearbyRoom != null)
        {
            canBePlaced = room.RoomEntrances.Any(x => x.RoomEntranceDirection == roomEntranceDirection && 
                nearbyRoom.RoomEntrances.Any(y => y.RoomEntranceDirection == neighboorEntranceDirection));
        }

        return canBePlaced;
    }

    private bool IsRoomPositionOccupied(Vector2Int position)
    {
        return spawnedRooms.Any(room => room.RoomInfo.Position == position);
    }
}
