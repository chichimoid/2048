using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Field
{
    public class GameFieldInputHandler : MonoBehaviour
    {
        private InputActions _inputActions;
        private InputAction _moveCellsAction;
    
        public static GameFieldInputHandler Instance;
        private void Awake()
        {
            Instance = this;
        
            _inputActions = new InputActions();
            _moveCellsAction = _inputActions.Player.MoveCells;
        }

        private void Start()
        {
            GameManager.Instance.OnGameEnded += OnDisable;
        }
    
        private void OnEnable()
        {
            _inputActions.Enable();
            _moveCellsAction.performed += HandleMoveCellsAction;
        }

        private void OnDisable()
        {
            _moveCellsAction.performed -= HandleMoveCellsAction;
            _inputActions.Disable();
        }

        private void HandleMoveCellsAction(InputAction.CallbackContext context)
        {
            var directionVector = context.ReadValue<Vector2>();
            Debug.Log($"Move cells action performed. Read value: {directionVector}");
            if (directionVector.x != 0)
            {
                OnMoveCellsInput?.Invoke(directionVector.x > 0 ? Direction.Right : Direction.Left);
            }
            else if (directionVector.y != 0)
            {
                OnMoveCellsInput?.Invoke(directionVector.y > 0 ? Direction.Up : Direction.Down);
            }
        }
    
        public delegate void OnMoveCellsInputDelegate(Direction direction);
        public event OnMoveCellsInputDelegate OnMoveCellsInput;
    }
}