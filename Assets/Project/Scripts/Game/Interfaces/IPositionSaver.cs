using System;
using UnityEngine;

namespace Project.Game.Interfaces
{
    public interface IPositionSaver
    {
        Vector3 SavedPosition { get; }
        bool HasPosition { get; }
        void SaveCurrentPosition();
        void RestorePosition();
        void RestorePositionAnimated(float duration = 0.5f, Action onComplete = null);
        void ClearSavedPosition();
    }
}