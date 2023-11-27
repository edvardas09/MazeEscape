using UnityEngine;

namespace MazeEscape.Gameplay.Characters
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] private Rigidbody m_rigidbody;
        [SerializeField] private float m_speed;
    
        public void Move(Vector2 direction)
        {
            if(direction == Vector2.zero)
            {
                m_rigidbody.velocity = Vector3.zero;
                return;
            }

            var moveDirection = new Vector3(direction.x, 0, direction.y);
            m_rigidbody.velocity = m_speed * moveDirection;
        }
    }
}