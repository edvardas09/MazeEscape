using UnityEngine;

namespace MazeEscape.Gameplay.Characters
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] private Rigidbody m_rigidbody;
        [SerializeField] private float m_speed;
        [SerializeField] private Animator m_animator;

        private Quaternion m_rotation;
        private Vector2 m_direction;
    
        public void Move(Vector2 direction)
        {
            var moveDirection = new Vector3(direction.x, 0, direction.y);
            if(!direction.Equals(m_direction) && direction.magnitude > 0.1f)
            {    
                m_rotation = Quaternion.LookRotation(moveDirection);
            }

            if(direction.magnitude < 0.1f && m_direction.magnitude > 0.1f)
            {
                m_animator.SetBool("IsRunning", false);
                m_rigidbody.velocity = Vector3.zero;
                m_direction = direction;
                return;
            }

            if(direction.magnitude > 0.1f && m_direction.magnitude < 0.1f)
            {
                m_animator.SetBool("IsRunning", true);
                m_animator.SetTrigger("StartRunning");      
            }

            m_rigidbody.velocity = m_speed * moveDirection;
            m_direction = direction;
        }

        private void Update()
        {
            // Quaternion.Lerp is not working properly when deltaTime is equal or greater than 1
            var deltaTime = Mathf.Min(Time.deltaTime * 10, 0.9f);

            var smoothRotation = Quaternion.Lerp(m_animator.transform.localRotation, m_rotation, deltaTime);

            // hacky way to stop the ik player animation from clipping through floor
            m_animator.transform.SetLocalPositionAndRotation(Vector3.zero, smoothRotation);
        }
    }
}