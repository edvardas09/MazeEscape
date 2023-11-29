using UnityEngine;

namespace MazeEscape.Sounds
{
    public class AlertFX : MonoBehaviour
    {
        [SerializeField] private float m_duration = 1f;
        [SerializeField] private float m_popoutTime = 0.75f;
        [SerializeField] private float m_popTime = 0.25f;
        [SerializeField] private float m_effectScale = 0.25f;

        private float m_startTime;

        private void Awake()
        {
            m_startTime = Time.realtimeSinceStartup;
        }

        private void Update()
        {
            // effect appearing animation using popTime
            if (Time.realtimeSinceStartup - m_startTime < m_popTime)
            {
                var scale = (Time.realtimeSinceStartup - m_startTime) / m_popTime;
                transform.localScale = new Vector3(scale, scale, scale) * m_effectScale;
            }
            else if (Time.realtimeSinceStartup - m_startTime > m_popoutTime)
            {
                var scale = 1f - (Time.realtimeSinceStartup - m_startTime - m_popoutTime) / (m_duration - m_popoutTime);
                transform.localScale = new Vector3(scale, scale, scale) * m_effectScale;
            }
           
            if (Time.realtimeSinceStartup - m_startTime > m_duration)
            {
                Destroy(gameObject);
            }
        }       
    }
}