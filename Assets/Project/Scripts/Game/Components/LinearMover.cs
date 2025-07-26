using Project.Game.Interfaces;
using UnityEngine;

namespace Project.Game.Components
{
    public class LinearMover : MonoBehaviour, IMovementController
    {
        [Header("Movement Settings")]
        [SerializeField] private Vector3 _velocity = Vector3.right;
        [SerializeField] private bool _moveOnStart = true;
        
        private bool _isMoving = false;

        public Vector3 Velocity => _velocity;
        public bool IsMoving => _isMoving;

        private void Start()
        {
            if (_moveOnStart)
            {
                StartMovement();
            }
        }

        private void Update()
        {
            if (_isMoving)
            {
                transform.position += _velocity * Time.deltaTime;
            }
        }

        public void StartMovement() => _isMoving = true;
        public void StopMovement() => _isMoving = false;
        public void SetVelocity(Vector3 newVelocity) => _velocity = newVelocity;
        public void SetSpeed(float speed)
        {
            if (_velocity != Vector3.zero)
            {
                _velocity = _velocity.normalized * speed;
            }
        }
    }
}