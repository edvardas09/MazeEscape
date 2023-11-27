using UnityEngine;

namespace MazeEscape.Gameplay
{
    public class CameraManager : MonoBehaviour
    {
        private const string PlayerTag = "Player";

        [SerializeField] private Camera m_gameplayCamera;
        [SerializeField] private Vector3 m_cameraOffset;

        private Transform m_playerTransform;

        protected void Start()
        {
            m_playerTransform = GameObject.FindGameObjectWithTag(PlayerTag).transform;
        }

        protected void Update()
        {
            UpdateCameraPosition();
        }

        private void UpdateCameraPosition()
        {
            m_gameplayCamera.transform.position = m_playerTransform.position + m_cameraOffset;
        }
    }
}
