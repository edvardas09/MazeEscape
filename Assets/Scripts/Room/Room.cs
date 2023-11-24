using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private List<RoomEntrance> roomEntrances = new();

    [Header("Prefabs")]
    [SerializeField] private GameObject startPrefab;
    [SerializeField] private GameObject endPrefab;

    public List<RoomEntrance> RoomEntrances => roomEntrances;
    public RoomInfo RoomInfo => roomInfo;

    private RoomInfo roomInfo = new();

    private void OnValidate()
    {
        roomEntrances = new List<RoomEntrance>(GetComponentsInChildren<RoomEntrance>());
    }

    public void Setup(RoomInfo roomInfo)
    {
        this.roomInfo = roomInfo;

        switch (roomInfo.RoomType)
        {
            case RoomType.Start:
                SpawnStartRoom();
                break;
            case RoomType.End:
                SpawnEndRoom();
                break;
            default:
                break;
        }
    }

    private void SpawnStartRoom()
    {
        Instantiate(startPrefab, transform);
    }

    private void SpawnEndRoom()
    {
        Instantiate(endPrefab, transform);
    }
}
