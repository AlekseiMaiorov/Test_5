using Project.Game.Core;
using Project.Game.Events;
using Project.Game.Interfaces;
using Project.Game.Shapes;
using Project.Inputs;
using UnityEngine;
using Zenject;

namespace Project.Game.Behaviours
{
    public class DefaultShapeBehaviour : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private DefaultShape _shape;
        [SerializeField] private Draggable _draggable;
        
        private IPositionSaver _positionSaver;
        private IMovementController _movementController;
        private IMatchChecker _matchChecker;
        private ISlotDetector _slotDetector;
        private SignalBus _signalBus;

        [Inject]
        private void Init(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _shape = GetComponent<DefaultShape>();
            _draggable = GetComponent<Draggable>();
            
            _positionSaver = GetComponent<IPositionSaver>();
            _movementController = GetComponent<IMovementController>();
            _matchChecker = GetComponent<IMatchChecker>();
            _slotDetector = GetComponent<ISlotDetector>();
        }

        private void Start()
        {
            SubscribeToEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        private void SubscribeToEvents()
        {
            if (!_draggable)
            {
                return;
            }

            _draggable.OnStartDrag.AddListener(HandleDragStart);
            _draggable.OnEndDrag.AddListener(HandleDragEnd);
        }

        private void UnsubscribeFromEvents()
        {
            if (!_draggable)
            {
                return;
            }

            _draggable.OnStartDrag.RemoveListener(HandleDragStart);
            _draggable.OnEndDrag.RemoveListener(HandleDragEnd);
        }

        private void HandleDragStart()
        {
            _positionSaver?.SaveCurrentPosition();
            _movementController?.StopMovement();
        }

        private void HandleDragEnd()
        {
            SortingSlot slot = _slotDetector?.FindSlotAt(transform.position);
            
            if (slot)
            {
                ProcessSlotInteraction(slot);
            }
            else
            {
                ReturnToOriginalPosition();
            }
        }

        private void ProcessSlotInteraction(SortingSlot slot)
        {
            MatchResult result = _matchChecker.CheckMatch(_shape, slot);
            
            if (result.IsMatch)
            {
                FireMatchEvent(slot, result);
            }
            else
            {
                FireMismatchEvent(slot, result);
            }
        }

        private void ReturnToOriginalPosition()
        {
            if (_positionSaver?.HasPosition == true)
            {
                _positionSaver.RestorePositionAnimated(0.5f, () => _movementController?.StartMovement());
            }
            else
            {
                _movementController?.StartMovement();
            }
        }

        private void FireMatchEvent(SortingSlot slot, MatchResult result)
        {
            _signalBus.Fire(new ShapeMatchedSignal
            {
                Shape = _shape,
                Slot = slot,
                Position = slot.transform.position,
                ScoreGained = result.ScoreValue,
                EffectId = result.EffectId,
                SoundId = result.SoundId
            });
        }

        private void FireMismatchEvent(SortingSlot slot, MatchResult result)
        {
            _signalBus.Fire(new ShapeMismatchedSignal
            {
                Shape = _shape,
                Slot = slot,
                Position = transform.position,
                HealthLost = result.HealthPenalty,
                EffectId = result.EffectId,
                SoundId = result.SoundId
            });
        }
    }
}