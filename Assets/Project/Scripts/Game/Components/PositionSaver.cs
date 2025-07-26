using System;
using DG.Tweening;
using Project.Game.Interfaces;
using UnityEngine;

namespace Project.Game.Components
{
    public class PositionSaver : MonoBehaviour, IPositionSaver
    {
        private Vector3 _savedPosition;
        private bool _hasPosition = false;

        public Vector3 SavedPosition => _savedPosition;
        public bool HasPosition => _hasPosition;

        public void SaveCurrentPosition()
        {
            _savedPosition = transform.position;
            _hasPosition = true;
        }

        public void RestorePosition()
        {
            if (_hasPosition)
            {
                transform.position = _savedPosition;
            }
        }

        public void RestorePositionAnimated(float duration = 0.5f, Action onComplete = null)
        {
            if (!_hasPosition) return;

            transform.DOMove(_savedPosition, duration)
                     .SetEase(Ease.OutBack)
                     .OnComplete(() => onComplete?.Invoke()).Restart();
        }

        public void ClearSavedPosition()
        {
            _hasPosition = false;
            _savedPosition = Vector3.zero;
        }
    }
}