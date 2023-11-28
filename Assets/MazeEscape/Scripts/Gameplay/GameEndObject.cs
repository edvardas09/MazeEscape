using UnityEngine;
using UnityEngine.Events;

namespace MazeEscape.Gameplay
{
    public class GameEndObject : MonoBehaviour
    {
        public UnityAction OnGameEnd;

        private const string c_playerTag = "Player";

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag(c_playerTag))
            {
                return;
            }

            OnGameEnd?.Invoke();
        }
    }
}