using DG.Tweening;
using Project.Game.Events;
using TMPro;
using UnityEngine;
using Zenject;

namespace Project.Game.UI
{
    public class GameUI : MonoBehaviour
    {
        private SignalBus _signalBus;
        
        [Header("UI Components")]
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _healthText;
        
        [Header("Formats")]
        [SerializeField] private string _scoreFormat = "Score: {0}";
        [SerializeField] private string _healthFormat = "Health: {0}";

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void Awake()
        {
            _signalBus.Subscribe<PlayerStatsChangedSignal>(OnPlayerStatsChanged);
        }

        private void OnDestroy()
        {
            _signalBus.TryUnsubscribe<PlayerStatsChangedSignal>(OnPlayerStatsChanged);
        }

        private void OnPlayerStatsChanged(PlayerStatsChangedSignal signal)
        {
            UpdateScoreDisplay(signal.Score);
            UpdateHealthDisplay(signal.Health);
            
            if (signal.ScoreDelta > 0)
            {
                AnimateScoreIncrease();
            }
            if (signal.HealthDelta < 0)
            {
                AnimateHealthDecrease();
            }
        }

        private void UpdateScoreDisplay(int score)
        {
            if (_scoreText)
            {
                _scoreText.text = string.Format(_scoreFormat, score);
            }
        }

        private void UpdateHealthDisplay(int health)
        {
            if (_healthText)
            {
                _healthText.text = string.Format(_healthFormat, health);
            }
        }

        private void AnimateScoreIncrease()
        {
            if (_scoreText)
            {
                _scoreText.transform.DOPunchScale(Vector3.one * 0.1f, 0.3f).Restart();
            }
        }

        private void AnimateHealthDecrease()
        {
            if (_healthText)
            {
                _healthText.transform.DOShakePosition(0.5f, Vector3.one * 10f).Restart();
            }
        }
    }
}