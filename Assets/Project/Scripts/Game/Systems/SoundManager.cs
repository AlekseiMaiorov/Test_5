using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Project.Game.Events;
using UnityEngine;
using Zenject;

namespace Project.Game.Systems
{
    public class SoundManager : MonoBehaviour
    {
        private SignalBus _signalBus;

        [SerializeField] private AudioSource _audioSource;

        [Header("Sounds")]
        [SerializeField]
        private SerializedDictionary<string, AudioClip> _sounds;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void Awake()
        {
            _signalBus.Subscribe<ShapeMatchedSignal>(OnShapeMatched);
            _signalBus.Subscribe<ShapeMismatchedSignal>(OnShapeMismatched);
            _signalBus.Subscribe<ShapeReachedDeathZoneSignal>(OnShapeReachedDeathZone);
        }

        private void OnDestroy()
        {
            _signalBus.TryUnsubscribe<ShapeMatchedSignal>(OnShapeMatched);
            _signalBus.TryUnsubscribe<ShapeMismatchedSignal>(OnShapeMismatched);
            _signalBus.TryUnsubscribe<ShapeReachedDeathZoneSignal>(OnShapeReachedDeathZone);
        }

        private void OnShapeMatched(ShapeMatchedSignal signal)
        {
            PlaySound(signal.SoundId);
        }

        private void OnShapeMismatched(ShapeMismatchedSignal signal)
        {
            PlaySound(signal.SoundId);
        }

        private void OnShapeReachedDeathZone(ShapeReachedDeathZoneSignal signal)
        {
            PlaySound(signal.SoundId);
        }

        private void PlaySound(string soundId)
        {
            if (_audioSource == null || string.IsNullOrEmpty(soundId))
                return;

            if (_sounds.TryGetValue(soundId, out var clip) && clip != null)
            {
                _audioSource.PlayOneShot(clip);
            }
#if UNITY_EDITOR
            else
            {
                Debug.LogWarning($"[SoundManager] Unknown sound id: '{soundId}'");
            }
#endif
        }
    }
}
