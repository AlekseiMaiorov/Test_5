using Project.Game.Events;
using UnityEngine;
using Zenject;

namespace Project.Game.Systems
{
    public class ScoreManager : MonoBehaviour
    {
        private SignalBus _signalBus;
        private int _currentScore = 0;
        
        public static int CurrentScore { get; private set; }

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void Awake()
        {
            _signalBus.Subscribe<ShapeMatchedSignal>(OnShapeMatched);
            _signalBus.Subscribe<RestartGameSignal>(OnRestartGame);
        }

        private void OnDestroy()
        {
            _signalBus.TryUnsubscribe<ShapeMatchedSignal>(OnShapeMatched);
            _signalBus.TryUnsubscribe<RestartGameSignal>(OnRestartGame);
        }

        private void OnShapeMatched(ShapeMatchedSignal signal)
        {
            int previousScore = _currentScore;
            _currentScore += signal.ScoreGained;
            CurrentScore = _currentScore;
            
            _signalBus.Fire(new PlayerStatsChangedSignal
            {
                Score = _currentScore,
                ScoreDelta = signal.ScoreGained,
                Health = HealthManager.CurrentHealth,
                HealthDelta = 0
            });
        }

        private void OnRestartGame()
        {
            ResetScore();
        }

        public void ResetScore()
        {
            _currentScore = 0;
            CurrentScore = 0;
        }
    }
}