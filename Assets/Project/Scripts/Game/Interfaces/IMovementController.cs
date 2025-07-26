using UnityEngine;

namespace Project.Game.Interfaces
{
    public interface IMovementController
    {
        bool IsMoving { get; }
        Vector3 Velocity { get; }
        void StartMovement();
        void StopMovement();
        void SetVelocity(Vector3 velocity);
        void SetSpeed(float speed);
    }
}