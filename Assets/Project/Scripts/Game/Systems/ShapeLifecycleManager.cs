using Project.Game.Core;
using Project.Game.Events;
using Project.Game.Pools;
using UnityEngine;
using Zenject;

namespace Project.Game.Systems
{
    public class ShapeLifecycleManager : MonoBehaviour
    {
        private SignalBus _signalBus;
        private ShapePool.Pool _shapePool;

        [Inject]
        private void Init(SignalBus signalBus, ShapePool.Pool shapePool)
        {
            _signalBus = signalBus;
            _shapePool = shapePool;
        }

        public void Awake()
        {
            _signalBus.Subscribe<ShapeMatchedSignal>(OnShapeMatched);
            _signalBus.Subscribe<ShapeMismatchedSignal>(OnShapeMismatched);
            _signalBus.Subscribe<ShapeReachedDeathZoneSignal>(OnShapeReachedEnd);
            _signalBus.Subscribe<ShapeDestroyedByExplosionSignal>(OnShapeDestroyedByExplosion);
            _signalBus.Subscribe<GameStateChangedSignal>(OnGameStateChanged);
        }

        private void OnDestroy()
        {
            _signalBus.TryUnsubscribe<ShapeMatchedSignal>(OnShapeMatched);
            _signalBus.TryUnsubscribe<ShapeMismatchedSignal>(OnShapeMismatched);
            _signalBus.TryUnsubscribe<ShapeReachedDeathZoneSignal>(OnShapeReachedEnd);
            _signalBus.TryUnsubscribe<ShapeDestroyedByExplosionSignal>(OnShapeDestroyedByExplosion);
            _signalBus.TryUnsubscribe<GameStateChangedSignal>(OnGameStateChanged);
        }

        private void OnShapeMatched(ShapeMatchedSignal signal)
        {
            if (!signal.Shape)
            {
                return;
            }

            signal.Shape.GetComponent<ShapePool>().ReturnToPool();
        }

        private void OnShapeMismatched(ShapeMismatchedSignal signal)
        {
            if (!signal.Shape)
            {
                return;
            }

            signal.Shape.GetComponent<ShapePool>().ReturnToPool();
        }

        private void OnShapeReachedEnd(ShapeReachedDeathZoneSignal signal)
        {
            if (!signal.Shape)
            {
                return;
            }

            signal.Shape.GetComponent<ShapePool>().ReturnToPool();
        }

        private void OnShapeDestroyedByExplosion(ShapeDestroyedByExplosionSignal signal)
        {
            if (!signal.Shape)
            {
                return;
            }

            signal.Shape.GetComponent<ShapePool>().ReturnToPool();
        }

        private void OnGameStateChanged(GameStateChangedSignal signal)
        {
            if (signal.NewState == GameState.GameOver ||
                signal.NewState == GameState.Victory)
            {
                _shapePool.DespawnAll();
            }
        }
    }
}