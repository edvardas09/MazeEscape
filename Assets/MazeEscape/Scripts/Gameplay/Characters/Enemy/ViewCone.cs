using UnityEngine;
using UnityEngine.Events;
using MazeEscape.Extensions;

namespace MazeEscape.Gameplay.Characters
{
    public class ViewCone : MonoBehaviour
    {
        public UnityAction<GameObject> OnPlayerFound;
        public UnityAction<GameObject> OnPlayerLost;

        private const string c_playerTag = "Player";
        private const string c_defaultTag = "Default";
        private const string c_playerLayer = "Player";

        [SerializeField] private Transform m_enemyTransform;
        [SerializeField] private MeshFilter m_meshFilter;
        [SerializeField] private MeshRenderer m_meshRenderer;

        private float m_distance;
        private float m_angle;
        private Color m_color;

        private Mesh m_mesh;
        private int m_layerMask;

        private GameObject m_playerObject;

        private void Start()
        {
            m_mesh = new Mesh();
            m_meshFilter.mesh = m_mesh;

            m_layerMask = LayerMask.GetMask(c_playerLayer, c_defaultTag);
        }

        private void Update()
        {
            GenerateMesh();
        }

        public void Setup(float distance, float angle, Color color)
        {
            m_distance = distance;
            m_angle = angle;
            m_color = color;
        }

        private void GenerateMesh()
        {
            var vertices = new Vector3[360 * 3];
            var triangles = new int[360 * 3];
            var colors = new Color[360 * 3];
            var triangleCount = 0;
            GameObject player = null;

            var currentWorldRotation = m_enemyTransform.rotation.eulerAngles.y + 90;
            currentWorldRotation = currentWorldRotation > 360 ? currentWorldRotation - 360 : currentWorldRotation;
            currentWorldRotation.NormalizeAngle();

            for (var angle = 0; angle < 360; angle++)
            {
                if (angle > currentWorldRotation + m_angle / 2 || angle < currentWorldRotation - m_angle / 2)
                {
                    continue;
                }
            
                var x = Mathf.Sin((angle - 90) * Mathf.Deg2Rad);
                var z = Mathf.Cos((angle - 90) * Mathf.Deg2Rad);

                var nextX = Mathf.Sin((angle + 1 - 90) * Mathf.Deg2Rad);
                var nextZ = Mathf.Cos((angle + 1 - 90) * Mathf.Deg2Rad);

                var hit = Physics.Raycast(transform.position, new Vector3(x, 0, z), out var hitInfo, m_distance, m_layerMask);

                float damageDistanceByRaycast;
                if (hit)
                {
                    if (!hitInfo.collider.CompareTag(c_playerTag))
                    {
                        damageDistanceByRaycast = Vector3.Distance(transform.position, hitInfo.point);
                    }
                    else
                    {
                        player = hitInfo.collider.gameObject;
                        damageDistanceByRaycast = (int)m_distance;
                    }
                }
                else
                {
                    damageDistanceByRaycast = (int)m_distance;
                }

                var firstPoint = new Vector3(x * damageDistanceByRaycast, 0, z * damageDistanceByRaycast);
                var nextPoint = new Vector3(nextX * damageDistanceByRaycast, 0, nextZ * damageDistanceByRaycast);

                vertices[triangleCount] = firstPoint;
                vertices[triangleCount + 1] = Vector3.zero;
                vertices[triangleCount + 2] = nextPoint;
                
                triangles[triangleCount] = triangleCount + 2;
                triangles[triangleCount + 1] = triangleCount + 1;
                triangles[triangleCount + 2] = triangleCount;

                triangleCount += 3;
            }

            m_mesh.vertices = vertices;
            m_mesh.triangles = triangles;

            m_meshRenderer.material.color = m_color;
            m_meshFilter.mesh = m_mesh;

            transform.rotation = Quaternion.identity;

            if (player == m_playerObject)
            {
                return;
            }

            m_playerObject = player;

            if (m_playerObject)
            {
                OnPlayerFound?.Invoke(m_playerObject);
            }
            else
            {
                OnPlayerLost?.Invoke(m_playerObject);
            }
        }
    }
}