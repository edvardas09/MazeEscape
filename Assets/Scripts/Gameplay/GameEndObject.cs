using UnityEngine;
using UnityEngine.Events;

namespace MazeEscape.Gameplay
{
    public class GameEndObject : MonoBehaviour
    {
        public UnityAction OnGameEnd;

        private const string PlayerTag = "Player";

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag(PlayerTag))
            {
                return;
            }

            OnGameEnd?.Invoke();
        }
    }
}