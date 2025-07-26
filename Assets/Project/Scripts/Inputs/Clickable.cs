using UnityEngine;
using UnityEngine.Events;

namespace Project.Inputs
{
    public sealed class Clickable : MonoBehaviour
    {
        [Header("Click Settings")]
        [SerializeField]
        private float _clickThreshold = 0.1f;

        [Header("Events")]
        public UnityEvent OnClick;
        public UnityEvent OnRelease;

        private bool _isPressed = false;
        private Vector3 _pressStartPosition;
        private float _pressStartTime;

        private void OnMouseDown()
        {
            _isPressed = true;
            _pressStartPosition = Input.mousePosition;
            _pressStartTime = Time.time;
        }

        private void OnMouseUp()
        {
            if (!_isPressed)
            {
                return;
            }

            _isPressed = false;
            
            OnRelease?.Invoke();
            
            Vector3 currentPosition = Input.mousePosition;
            float distance = Vector3.Distance(_pressStartPosition, currentPosition);

            if (distance <= _clickThreshold)
            {
                OnClick?.Invoke();
            }
        }

        private void OnMouseExit()
        {
            if (!_isPressed)
            {
                return;
            }

            _isPressed = false;
            OnRelease?.Invoke();
        }
       
        public bool IsPressed()
        {
            return _isPressed;
        }

        public float GetPressDuration()
        {
            return _isPressed ? Time.time - _pressStartTime : 0f;
        }
    }
}