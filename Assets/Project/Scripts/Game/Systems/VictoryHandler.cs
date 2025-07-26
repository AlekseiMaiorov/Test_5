using System;
using Project.Game.Core;
using Project.Game.Events;
using Project.Game.Pools;
using Zenject;

namespace Project.Game.Systems
{
    public class VictoryHandler : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly ShapePool.Pool _shapePool;
        private bool _spawningFinished;

        public VictoryHandler(SignalBus signalBus, ShapePool.Pool shapePool)
        {
            _signalBus = signalBus;
            _shapePool = shapePool;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<ShapeSpawnerStoppedSignal>(OnStopSpawning);
            _signalBus.Subscribe<RestartGameSignal>(OnRestartGame);

            _signalBus.Subscribe<ShapeMatchedSignal>(OnShapeResolved);
            _signalBus.Subscribe<ShapeMismatchedSignal>(OnShapeResolved);
            _signalBus.Subscribe<ShapeReachedDeathZoneSignal>(OnShapeResolved);
            _signalBus.Subscribe<ShapeDestroyedByExplosionSignal>(OnShapeResolved);
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<ShapeSpawnerStoppedSignal>(OnStopSpawning);
            _signalBus.TryUnsubscribe<RestartGameSignal>(OnRestartGame);

            _signalBus.TryUnsubscribe<ShapeMatchedSignal>(OnShapeResolved);
            _signalBus.TryUnsubscribe<ShapeMismatchedSignal>(OnShapeResolved);
            _signalBus.TryUnsubscribe<ShapeReachedDeathZoneSignal>(OnShapeResolved);
            _signalBus.TryUnsubscribe<ShapeDestroyedByExplosionSignal>(OnShapeResolved);
        }

        private void OnStopSpawning()
        {
            _spawningFinished = true;
            TryCheckVictory();
        }

        private void OnRestartGame()
        {
            _spawningFinished = false;
        }

        private void OnShapeResolved<T>(T _)
        {
            TryCheckVictory();
        }

        private void TryCheckVictory()
        {
            if (!_spawningFinished)
                return;

            if (_shapePool.ActiveCount == 0 && HealthManager.CurrentHealth > 0)
            {
                _signalBus.Fire(new GameStateChangedSignal
                {
                    NewState = GameState.Victory,
                    PreviousState = GameState.Playing
                });
            }
        }
    }
}
