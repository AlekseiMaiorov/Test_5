using Project.Game.Shapes;
using UnityEngine;

namespace Project.Game.Core
{
    public class SortingSlot : MonoBehaviour
    {
        [Header("Slot Settings")]
        [SerializeField]
        private ShapeData _data;
        [SerializeField]
        private SpriteRenderer _sprite;

        public ShapeData Data => _data;
        public int Id => _data?.Id ?? -1;

#if UNITY_EDITOR
        [ContextMenu("Apply Sprite")]
        private void ApplySprite()
        {
            _sprite.sprite = Data.Sprite;
        }
#endif
    }
}