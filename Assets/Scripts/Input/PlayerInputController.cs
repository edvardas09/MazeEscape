using UnityEngine;
using UnityEngine.InputSystem;
using MazeEscape.Gameplay.Character;

namespace MazeEscape.Input
{
    public class PlayerInputController : MonoBehaviour
    {
        private const string m_actionMapName = "Player";
        private const string m_moveActionName = "Move";

        [SerializeField] private PlayerInput m_playerInput;
        [SerializeField] private Movement m_movement;

        private InputMaster m_inputMaster;
        private InputActionMap m_actionMap;
        private InputAction m_moveAction;

        protected void OnEnable()
        {
            Initialize();
        }

        protected void OnDisable()
        {
            m_inputMaster.Player.Disable();
        }

        private void Initialize()
        {
            m_inputMaster = new();
            m_inputMaster.Player.Enable();

            m_actionMap = m_playerInput.actions.FindActionMap(m_actionMapName);
            m_moveAction = m_actionMap.FindAction(m_moveActionName);
        }

        private void Update() 
        {
            UpdateMovement();
        }

        private void UpdateMovement()
        {
            var movementVector = GetMovementVector();
            movementVector = Quaternion.Euler(0, 0, 45) * movementVector;


            m_movement.Move(movementVector);
        }

        private Vector2 GetMovementVector()
        {
            return m_moveAction.ReadValue<Vector2>();
        }
    }
}