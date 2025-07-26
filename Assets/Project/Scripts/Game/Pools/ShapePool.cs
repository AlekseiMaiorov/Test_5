using System.Collections.Generic;
using Project.Game.Shapes;
using UnityEngine;
using Zenject;

namespace Project.Game.Pools
{
    public class ShapePool : MonoBehaviour, IPoolable<ShapeData, Vector3, IMemoryPool>
    {
        [Header("Shape Components")]
        [SerializeField]
        private DefaultShape _shape;

        private ShapeData _currentData;
        private IMemoryPool _pool;

        public DefaultShape Shape => _shape;

        private void Awake()
        {
            _shape = GetComponent<DefaultShape>();
        }

        public void OnDespawned()
        {
            _pool = null;
            _currentData = null;
            gameObject.SetActive(false);
        }

        public void OnSpawned(ShapeData shapeData, Vector3 position, IMemoryPool pool)
        {
            gameObject.SetActive(true);
            _currentData = shapeData;
            _pool = pool;

            transform.position = position;
            gameObject.SetActive(true);

            if (_shape)
            {
                _shape.Initialize(shapeData);
            }
        }

        public void ReturnToPool()
        {
            _pool?.Despawn(this);
        }

        public class Pool : MemoryPool<ShapeData, Vector3, ShapePool>
        {
            private readonly List<ShapePool> _activeShapes = new List<ShapePool>();

            protected override void OnCreated(ShapePool item)
            {
                item.gameObject.SetActive(false);
            }

            protected override void OnSpawned(ShapePool item)
            {
                _activeShapes.Add(item);
            }

            protected override void OnDespawned(ShapePool item)
            {
                item.OnDespawned();
                _activeShapes.Remove(item);
            }

            protected override void Reinitialize(ShapeData shapeData, Vector3 position, ShapePool item)
            {
                base.Reinitialize(shapeData, position, item);
                item.OnSpawned(shapeData, position, this);
            }

            public void DespawnAll()
            {
                var shapesToDespawn = new List<ShapePool>(_activeShapes);

                foreach (var shape in shapesToDespawn)
                {
                    if (shape != null &&
                        shape.gameObject.activeInHierarchy)
                    {
                        Despawn(shape);
                    }
                }

                _activeShapes.Clear();
            }

            public int ActiveCount => _activeShapes.Count;
        }
    }
}