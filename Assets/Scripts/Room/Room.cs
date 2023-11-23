using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private List<RoomEntrance> roomEntrances = new();
    [SerializeField] private RoomType roomType;

    [Header("Prefabs")]
    [SerializeField] private GameObject startPrefab;
    [SerializeField] private GameObject endPrefab;

    public List<RoomEntrance> RoomEntrances => roomEntrances;

    private void OnValidate()
    {
        if (roomEntrances.Count == 0)
        {
            roomEntrances = new List<RoomEntrance>(GetComponentsInChildren<RoomEntrance>());
        }

        if (roomType == RoomType.Start && startPrefab == null)
        {
            Debug.LogError("Start prefab is not set", this);
        }

        if (roomType == RoomType.End && endPrefab == null)
        {
            Debug.LogError("End prefab is not set", this);
        }
    }

    private void Awake()
    {
        switch (roomType)
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

    public void SpawnStartRoom()
    {
        if (roomType != RoomType.Start)
        {
            Debug.LogError("Room is not a start room", this);
            return;
        }

        Instantiate(startPrefab, transform);
    }

    public void SpawnEndRoom()
    {
        if (roomType != RoomType.End)
        {
            Debug.LogError("Room is not an end room", this);
            return;
        }

        Instantiate(endPrefab, transform);
    }
}
