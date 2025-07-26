using DG.Tweening;
using Project.Game.Core;
using Project.Game.Events;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Game.UI
{
    public class GameOverPopup : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField] private GameObject _popupPanel;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Button _restartButton;

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
            if (signal.NewState == GameState.GameOver)
                Show();
        }

        private void Show()
        {
            _popupPanel.SetActive(true);
            _canvasGroup.alpha = 0;
            _popupPanel.transform.localScale = Vector3.zero;

            _canvasGroup.DOFade(1f, 0.4f).Restart();
            _popupPanel.transform.DOScale(Vector3.one, 0.4f).SetEase(Ease.OutBack).Restart();
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
            _popupPanel.transform.DOScale(Vector3.zero, 0.3f)
                .SetEase(Ease.InBack)
                .OnComplete(() => _popupPanel.SetActive(false)).Restart();
        }

        private void OnRestartClicked()
        {
            Hide();
            DOVirtual.DelayedCall(0.3f, () => _signalBus.Fire<RestartGameSignal>()).Restart();
        }
    }
}
