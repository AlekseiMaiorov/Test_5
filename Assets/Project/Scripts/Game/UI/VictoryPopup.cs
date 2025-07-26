using DG.Tweening;
using Project.Game.Core;
using Project.Game.Events;
using Project.Game.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Game.UI
{
    public class VictoryPopup : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField]
        private GameObject _popupPanel;
        [SerializeField]
        private CanvasGroup _canvasGroup;
        [SerializeField]
        private TextMeshProUGUI _scoreText;
        [SerializeField]
        private Button _restartButton;

        private SignalBus _signalBus;

        [Inject]
        private void Init(SignalBus signalBus) => _signalBus = signalBus;

        public void Awake()
        {
            _signalBus.Subscribe<GameStateChangedSignal>(OnGameStateChanged);
            _restartButton.onClick.AddListener(OnRestartClicked);
            Hide(immediate: true);
        }

        private void OnDestroy()
        {
            _signalBus.TryUnsubscribe<GameStateChangedSignal>(OnGameStateChanged);
        }

        private void OnGameStateChanged(GameStateChangedSignal signal)
        {
            if (signal.NewState == GameState.Victory)
                Show();
        }

        private void Show()
        {
            _popupPanel.SetActive(true);
            _canvasGroup.alpha = 0;
            _popupPanel.transform.localScale = Vector3.zero;
            _restartButton.transform.localScale = Vector3.zero;

            _canvasGroup.DOFade(1f, 0.5f).SetEase(Ease.OutQuad).Restart();
            _popupPanel.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack)
                       .OnComplete(AnimateScore).Restart();
        }

        private void AnimateScore()
        {
            int finalScore = ScoreManager.CurrentScore;

            DOVirtual.Int(0,
                          finalScore,
                          1f,
                          value => { _scoreText.text = $"Набрано очков: {value}"; })
                     .SetEase(Ease.OutQuart).Restart();

            _restartButton.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack).Restart();
        }

        private void OnRestartClicked()
        {
            Hide();
            _signalBus.Fire(new RestartGameSignal());
        }

        private void Hide(bool immediate = false)
        {
            if (immediate)
            {
                _popupPanel.SetActive(false);
                _canvasGroup.alpha = 0;
                _popupPanel.transform.localScale = Vector3.zero;
                return;
            }

            _canvasGroup.DOFade(0f, 0.3f).Restart();

            _popupPanel.transform.DOScale(0f, 0.3f).SetEase(Ease.InBack)
                       .OnComplete(() => _popupPanel.SetActive(false)).Restart();
        }
    }
}