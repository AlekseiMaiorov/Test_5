using Project.Game.Events;
using Project.Game.Interfaces;
using Project.Game.Shapes;
using UnityEngine;
using Zenject;

namespace Project.Game.Components
{
    public class ShapeDeathZoneHandler : MonoBehaviour, IDeathZoneHandler
    {
        private SignalBus _signalBus;
        private DefaultShape _shape;

        [Inject]
        private void Init(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _shape = GetComponent<DefaultShape>();
        }

        public void OnReachedDeathZone()
        {
            if (_shape)
            {
                _signalBus.Fire(new ShapeReachedDeathZoneSignal
                {
                    Shape = _shape,
                    Position = transform.position,
                    HealthLost = _shape.Data.HealthPenalty,
                    EffectId = _shape.Data.ExplosionEffectId,
                    SoundId = _shape.Data.ExplosionSoundId
                });
            }
        }
    }
}