using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Game.Components;
using Project.Game.Core;
using Project.Game.Events;
using Project.Game.Interfaces;
using Project.Game.Settings;
using Project.Game.Shapes;
using Project.Game.Systems;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Project.Game.Spawners
{
    public class ShapeSpawner : MonoBehaviour
    {
        private SignalBus _signalBus;
        private IShapeFactory _shapeFactory;
        private ISortingSlotsProvider _slotsProvider;
        private GameSettings _gameSettings;

        [SerializeField]
        private Transform[] _spawnPoints = new Transform[3];

        private int _totalShapesToSpawn;
        private int _shapesSpawned;
        private CancellationTokenSource _cancellationTokenSource;
        private List<ShapeData> _availableShapeData = new();

        [Inject]
        private void Init(
            SignalBus signalBus,
            IShapeFactory shapeFactory,
            ISortingSlotsProvider slotsProvider,
            GameSettings gameSettings)
        {
            _signalBus = signalBus;
            _shapeFactory = shapeFactory;
            _slotsProvider = slotsProvider;
            _gameSettings = gameSettings;
        }

        private void Awake()
        {
            SubscribeToSignals();
            InitializeShapeData();
            InitializeSpawnCount();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        private void OnDestroy()
        {
            UnsubscribeFromSignals();
            CancelSpawning();
        }

        private void SubscribeToSignals()
        {
            _signalBus.Subscribe<StartSpawnShapesSignal>(OnStartSpawnShapes);
            _signalBus.Subscribe<StopSpawnShapesSignal>(OnStopSpawnShapes);
            _signalBus.Subscribe<RestartGameSignal>(OnRestartGame);
        }

        private void UnsubscribeFromSignals()
        {
            _signalBus.TryUnsubscribe<StartSpawnShapesSignal>(OnStartSpawnShapes);
            _signalBus.TryUnsubscribe<StopSpawnShapesSignal>(OnStopSpawnShapes);
            _signalBus.TryUnsubscribe<RestartGameSignal>(OnRestartGame);
        }

        private void InitializeShapeData()
        {
            _availableShapeData = _slotsProvider.GetAvailableShapeData();
        }

        private void InitializeSpawnCount()
        {
            _totalShapesToSpawn =
                Mathf.RoundToInt(Random.Range(_gameSettings.ShapesToSpawnRange.x, _gameSettings.ShapesToSpawnRange.y));
            _shapesSpawned = 0;
        }

        private void OnStartSpawnShapes()
        {
            CancelSpawning();
            _cancellationTokenSource = new CancellationTokenSource();
            StartSpawningAsync(_cancellationTokenSource.Token).Forget();
        }

        private void OnStopSpawnShapes()
        {
            CancelSpawning();
        }

        private void OnRestartGame()
        {
            CancelSpawning();
            InitializeSpawnCount();
        }

        private void CancelSpawning()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        private async UniTaskVoid StartSpawningAsync(CancellationToken cancellationToken)
        {
            while (_shapesSpawned < _totalShapesToSpawn &&
                   !cancellationToken.IsCancellationRequested)
            {
                SpawnRandomShape();

                float delay = Random.Range(_gameSettings.SpawnTimeoutRange.x, _gameSettings.SpawnTimeoutRange.y);
                await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: cancellationToken);

                if (cancellationToken.IsCancellationRequested)
                    break;
            }

            if (!cancellationToken.IsCancellationRequested &&
                _shapesSpawned >= _totalShapesToSpawn)
            {
                _signalBus.Fire(new ShapeSpawnerStoppedSignal());
            }
        }

        private void SpawnRandomShape()
        {
            if (_spawnPoints.Length == 0 ||
                _availableShapeData.Count == 0)
                return;

            int laneIndex = Random.Range(0, _spawnPoints.Length);
            Transform spawnPoint = _spawnPoints[laneIndex];

            if (!spawnPoint)
                return;

            ShapeData randomShapeData = GetRandomShapeData();
            var spawned = _shapeFactory.CreateShape(randomShapeData, spawnPoint.position);

            if (spawned?.Shape == null)
                return;

            _shapesSpawned++;
            SetupShapeMovement(spawned.Shape);
        }

        private ShapeData GetRandomShapeData()
        {
            int randomIndex = Random.Range(0, _availableShapeData.Count);
            return _availableShapeData[randomIndex];
        }

        private void SetupShapeMovement(DefaultShape shape)
        {
            var linearMover = shape.GetComponent<LinearMover>();
            if (linearMover)
            {
                float randomSpeed = Random.Range(_gameSettings.ShapeSpeedRange.x, _gameSettings.ShapeSpeedRange.y);
                linearMover.SetSpeed(randomSpeed);
                linearMover.StartMovement();
            }
        }
    }
}