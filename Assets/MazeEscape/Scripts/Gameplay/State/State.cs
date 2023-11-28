using UnityEngine;
using UnityEngine.AI;

namespace MazeEscape.Gameplay.State
{
    public abstract class State : ScriptableObject
    {
        protected StateMachine m_ownerStateMachine;
        protected NavMeshAgent m_agent;

        protected Transform Transform => m_agent.transform;

        public virtual void OnInit(StateMachine stateMachine, NavMeshAgent agent)
        {
            m_ownerStateMachine = stateMachine;
            m_agent = agent;
        }

        public abstract void OnStart();
        public abstract void OnExit();
        public abstract void OnUpdate();
    }
}