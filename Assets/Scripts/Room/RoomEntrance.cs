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
}
