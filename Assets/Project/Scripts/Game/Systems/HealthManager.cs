using System;
using Project.Game.Core;
using Project.Game.Events;
using Project.Game.Settings;
using UnityEngine;
using Zenject;

namespace Project.Game.Systems
{
    public class HealthManager : MonoBehaviour
    {
        private SignalBus _signalBus;
        private GameSettings _gameSettings;
        private int _currentHealth;

        public static int CurrentHealth { get; private set; }

        [Inject]
        private void Construct(SignalBus signalBus, GameSettings gameSettings)
        {
            _signalBus = signalBus;
            _gameSettings = gameSettings;
        }

        public void Awake()
        {
            _currentHealth = _gameSettings.PlayerHealth;
            CurrentHealth = _currentHealth;

            _signalBus.Subscribe<ShapeMismatchedSignal>(OnShapeMismatched);
            _signalBus.Subscribe<ShapeReachedDeathZoneSignal>(OnShapeReachedEnd);
            _signalBus.Subscribe<RestartGameSignal>(OnRestartGame);
        }

        private void Start()
        {
            _signalBus.Fire(new PlayerStatsChangedSignal
            {
                Health = _currentHealth
            });
        }

        private void OnDestroy()
        {
            _signalBus.TryUnsubscribe<ShapeMismatchedSignal>(OnShapeMismatched);
            _signalBus.TryUnsubscribe<ShapeReachedDeathZoneSignal>(OnShapeReachedEnd);
            _signalBus.TryUnsubscribe<RestartGameSignal>(OnRestartGame);
        }

        private void OnShapeMismatched(ShapeMismatchedSignal signal)
        {
            TakeDamage(signal.HealthLost);
        }

        private void OnShapeReachedEnd(ShapeReachedDeathZoneSignal signal)
        {
            TakeDamage(signal.HealthLost);
        }

        private void OnRestartGame()
        {
            ResetHealth();
        }

        private void TakeDamage(int damage)
        {
            int previousHealth = _currentHealth;
            _currentHealth = Mathf.Max(0, _currentHealth - damage);
            CurrentHealth = _currentHealth;

            _signalBus.Fire(new PlayerStatsChangedSignal
            {
                Health = _currentHealth,
                HealthDelta = _currentHealth - previousHealth,
                Score = ScoreManager.CurrentScore,
                ScoreDelta = 0
            });

            if (_currentHealth <= 0)
            {
                _signalBus.Fire(new GameStateChangedSignal
                {
                    NewState = GameState.GameOver,
                    PreviousState = GameState.Playing
                });
            }
        }

        public void ResetHealth()
        {
            _currentHealth = _gameSettings.PlayerHealth;
            CurrentHealth = _currentHealth;
        }
    }
}