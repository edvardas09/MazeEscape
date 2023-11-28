using MazeEscape.Gameplay.Characters;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace MazeEscape.Gameplay.State
{
    public class StateMachine : MonoBehaviour
    {
        [SerializeField] private Enemy m_enemy;
        [SerializeField] private NavMeshAgent m_agent;
        [SerializeField] private float m_viewDistanceCircle = 2.5f;
        [SerializeField] private State m_currentState;
        [SerializeField] private MazeGenerator.MazeGenerator m_mazeGenerator;
        [SerializeField] private Animator m_animator;

        public UnityAction OnStateChange;

        public Vector3 TargetPosition => m_targetPosition;
        public MazeGenerator.MazeGenerator MazeGenerator => m_mazeGenerator;
        public Animator Animator => m_animator;
        public Enemy Enemy => m_enemy;
        public float ViewDistanceFront => m_viewDistanceCone;
        public float ViewDistanceBack => m_viewDistanceCircle;
        public int ViewAngle => m_viewAngle;
        public State CurrentState => m_currentState;

        private float m_viewDistanceCone;
        private int m_viewAngle;

        private Vector3 m_targetPosition;

        public void Initialize(MazeGenerator.MazeGenerator mazeGenerator)
        {
            m_mazeGenerator = mazeGenerator;
        }

        public void Setup(float viewDistance, int viewAngle)
        {
            m_viewDistanceCone = viewDistance;
            m_viewAngle = viewAngle;
        }

        private void Start()
        {
            SetState<Patrolling>();
        }

        private void Update()
        {
            m_currentState.OnUpdate();

            m_animator.transform.localPosition = Vector3.zero;
        }

        public void SetState<T>() where T : State
        {
            if(m_currentState)
                m_currentState.OnExit();

            var state = ScriptableObject.CreateInstance<T>();
            state.OnInit(this, m_agent);
            state.OnStart();

            m_currentState = state;

            OnStateChange?.Invoke();
        }
    }
}