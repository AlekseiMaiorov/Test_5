using UnityEngine;
using Zenject;

namespace Project.Game.Shapes
{
    public class DefaultShape : MonoBehaviour
    {
        [SerializeField] private ShapeData _data;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        private SignalBus _signalBus;
        
        public ShapeData Data => _data;
        public int Id => _data?.Id ?? -1;
        
        public void Initialize(ShapeData data)
        {
            _data = data;
            UpdateVisuals();
        }
        
        private void UpdateVisuals()
        {
            if (_spriteRenderer && _data)
            {
                _spriteRenderer.sprite = _data.Sprite;
            }
        }
    }
}