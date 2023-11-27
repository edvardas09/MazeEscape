using UnityEngine;
using UnityEngine.AI;

namespace MazeEscape.Gameplay.State
{
    public class StateMachine : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent m_agent;
        [SerializeField] private float m_viewDistanceCircle = 2.5f;

        public Vector3 TargetPosition => m_targetPosition;
        public MazeGenerator.MazeGenerator MazeGenerator => m_mazeGenerator;
        
        public float ViewDistanceFront => m_viewDistanceCone;
        public float ViewDistanceBack => m_viewDistanceCircle;
        public int ViewAngle => m_viewAngle;

        private float m_viewDistanceCone;
        private int m_viewAngle;

        private State m_currentState;
        private Vector3 m_targetPosition;
        private MazeGenerator.MazeGenerator m_mazeGenerator;

        public void Initialize(MazeGenerator.MazeGenerator mazeGenerator)
        {
            this.m_mazeGenerator = mazeGenerator;
        }

        public void Setup(float viewDistance, int viewAngle)
        {
            this.m_viewDistanceCone = viewDistance;
            this.m_viewAngle = viewAngle;
        }

        private void Start()
        {
            SetState<Patrolling>();
        }

        private void Update()
        {
            m_currentState.OnUpdate();
        }

        public void SetState<T>() where T : State
        {
            if(m_currentState)
                m_currentState.OnExit();

            var state = ScriptableObject.CreateInstance<T>();
            state.OnInit(this, m_agent);
            state.OnStart();

            m_currentState = state;
        }
    }
}