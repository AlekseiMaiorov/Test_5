using UnityEngine;
using UnityEngine.Events;

namespace Project.Inputs
{
    public sealed class Draggable : MonoBehaviour
    {
        [Header("Drag Settings")]
        [SerializeField]
        private float _dragThreshold = 0.1f;

        [Header("Events")]
        public UnityEvent OnStartDrag;
        public UnityEvent OnDrag;
        public UnityEvent OnEndDrag;

        private bool _isDragging = false;
        private bool _isPressed = false;
        private Vector3 _offset;
        private Vector3 _pressStartPosition;
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        private void OnMouseDown()
        {
            _isPressed = true;
            _pressStartPosition = Input.mousePosition;

            Vector3 worldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            worldPosition.z = transform.position.z;
            _offset = transform.position - worldPosition;
        }

        private void OnMouseDrag()
        {
            if (!_isPressed)
            {
                return;
            }
            
            if (!_isDragging)
            {
                Vector3 currentPosition = Input.mousePosition;
                float distance = Vector3.Distance(_pressStartPosition, currentPosition);

                if (distance >= _dragThreshold)
                {
                    _isDragging = true;
                    OnStartDrag?.Invoke();
                }
                else
                {
                    return;
                }
            }
            
            Vector3 targetPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            targetPosition.z = transform.position.z;
            targetPosition += _offset;

            transform.position = targetPosition;
            OnDrag?.Invoke();
        }

        private void OnMouseUp()
        {
            if (_isDragging)
            {
                _isDragging = false;
                OnEndDrag?.Invoke();
            }

            _isPressed = false;
        }
        
        public bool IsDragging()
        {
            return _isDragging;
        }

        public bool IsPressed()
        {
            return _isPressed;
        }

        public void StopDrag()
        {
            if (_isDragging)
            {
                _isDragging = false;
                OnEndDrag?.Invoke();
            }

            _isPressed = false;
        }
    }
}