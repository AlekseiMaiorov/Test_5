using System;
using Project.Game.Events;
using UnityEngine;
using Zenject;

namespace Project.Game.Core
{
    public class GameController : MonoBehaviour
    {
        private SignalBus _signalBus;

        [Inject]
        private void Init(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _signalBus.Subscribe<RestartGameSignal>(OnRestartGame);
            _signalBus.Subscribe<GameStateChangedSignal>(OnGameStateChanged);
        }

        public void Start()
        {
            StartGame();
        }

        private void OnDestroy()
        {
            _signalBus.TryUnsubscribe<RestartGameSignal>(OnRestartGame);
            _signalBus.TryUnsubscribe<GameStateChangedSignal>(OnGameStateChanged);
        }

        private void StartGame()
        {
            _signalBus.Fire<StartSpawnShapesSignal>();

            _signalBus.Fire(new GameStateChangedSignal
            {
                NewState = GameState.Playing,
                PreviousState = GameState.Loading
            });
        }

        private void OnRestartGame()
        {
            _signalBus.Fire<StopSpawnShapesSignal>();

            Invoke(nameof(StartGame), 0.1f);
        }

        private void OnGameStateChanged(GameStateChangedSignal signal)
        {
            if (signal.NewState == GameState.GameOver || signal.NewState == GameState.Victory)
            {
                _signalBus.Fire<StopSpawnShapesSignal>();
            }
        }

        [ContextMenu("Start Game")]
        private void DebugStartGame()
        {
            StartGame();
        }

        [ContextMenu("Stop Game")]
        private void DebugStopGame()
        {
            _signalBus.Fire<StopSpawnShapesSignal>();
        }

        [ContextMenu("Restart Game")]
        private void DebugRestartGame()
        {
            _signalBus.Fire<RestartGameSignal>();
        }
    }
}