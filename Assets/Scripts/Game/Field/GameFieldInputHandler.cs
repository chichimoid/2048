using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.Serialization;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace Game.Field
{
    public class GameFieldInputHandler : MonoBehaviour
    {
        private InputActions _inputActions;
        private InputAction _moveCellsAction;
        
        private Vector2 _startPos;
        private bool _isTracking;
        
        [SerializeField] private float minSwipeDistance = 50f;
        [SerializeField] private float minMouseDrag = 50f;

        public static GameFieldInputHandler Instance;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            
            _inputActions = new InputActions();
            _moveCellsAction = _inputActions.Player.MoveCells;

            EnhancedTouchSupport.Enable();
        }

        private void Start()
        {
            GameManager.Instance.OnGameEnded += OnDisable;
        }
    
        private void OnEnable()
        {
            _inputActions.Enable();
            _moveCellsAction.performed += HandleMoveCellsAction;
            Touch.onFingerDown += HandleFingerDown;
            Touch.onFingerUp += HandleFingerUp;
            
            _inputActions.Player.MouseClick.performed += HandleMouseDown;
            _inputActions.Player.MouseClick.canceled += HandleMouseUp;
        }

        private void OnDisable()
        {
            _moveCellsAction.performed -= HandleMoveCellsAction;
            _inputActions.Disable();
            
            Touch.onFingerDown -= HandleFingerDown;
            Touch.onFingerUp -= HandleFingerUp;
            
            GameManager.Instance.OnGameEnded -= OnDisable;
        }

        private void HandleMoveCellsAction(InputAction.CallbackContext context)
        {
            var directionVector = context.ReadValue<Vector2>();
            Debug.Log($"Move cells action performed. Read value: {directionVector}");
            ProcessDirectionInput(directionVector);
        }
        
        private void HandleFingerDown(Finger finger)
        {
            if (Touch.activeTouches.Count == 1)
            {
                _startPos = finger.screenPosition;
                _isTracking = true;
            }
        }
        private void HandleMouseDown(InputAction.CallbackContext context)
        {
            _startPos = Mouse.current.position.ReadValue();
            _isTracking = true;
        }

        private void HandleFingerUp(Finger finger)
        {
            if (!_isTracking) return;
            
            Vector2 touchEndPos = finger.screenPosition;
            Vector2 swipeDelta = touchEndPos - _startPos;
            
            if (swipeDelta.magnitude >= minSwipeDistance)
            {
                ProcessDirectionInput(swipeDelta.normalized);
            }
            
            _isTracking = false;
        }
        
        private void HandleMouseUp(InputAction.CallbackContext context)
        {
            if (!_isTracking) return;
            
            Vector2 endPos = Mouse.current.position.ReadValue();
            Vector2 delta = endPos - _startPos;
            
            if (delta.magnitude >= minMouseDrag)
            {
                ProcessDirectionInput(delta.normalized);
            }
            
            _isTracking = false;
        }
        
        private void ProcessDirectionInput(Vector2 directionVector)
        {
            if (Mathf.Abs(directionVector.x) > Mathf.Abs(directionVector.y))
            {
                OnMoveCellsInput?.Invoke(directionVector.x > 0 ? Direction.Right : Direction.Left);
            }
            else
            {
                OnMoveCellsInput?.Invoke(directionVector.y > 0 ? Direction.Up : Direction.Down);
            }
        }
    
        public delegate void OnMoveCellsInputDelegate(Direction direction);
        public event OnMoveCellsInputDelegate OnMoveCellsInput;
    }
}