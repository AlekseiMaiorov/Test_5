using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Project.Game.Events;
using UnityEngine;
using Zenject;

namespace Project.Game.Systems
{
    public class EffectsManager : MonoBehaviour
    {
        private SignalBus _signalBus;

        [Header("Effect Prefabs")]
        [SerializedDictionary]
        [SerializeField]
        private SerializedDictionary<string, GameObject> _effectPrefabs = new SerializedDictionary<string, GameObject>();

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void Awake()
        {
            _signalBus.Subscribe<ShapeMatchedSignal>(OnShapeMatched);
            _signalBus.Subscribe<ShapeMismatchedSignal>(OnShapeMismatched);
            _signalBus.Subscribe<ShapeReachedDeathZoneSignal>(OnShapeReachedEnd);
        }

        private void OnDestroy()
        {
            _signalBus.TryUnsubscribe<ShapeMatchedSignal>(OnShapeMatched);
            _signalBus.TryUnsubscribe<ShapeMismatchedSignal>(OnShapeMismatched);
            _signalBus.TryUnsubscribe<ShapeReachedDeathZoneSignal>(OnShapeReachedEnd);
        }

        private void OnShapeMatched(ShapeMatchedSignal signal)
        {
            PlayEffect(signal.EffectId, signal.Position);
        }

        private void OnShapeMismatched(ShapeMismatchedSignal signal)
        {
            PlayEffect(signal.EffectId, signal.Position);
        }

        private void OnShapeReachedEnd(ShapeReachedDeathZoneSignal signal)
        {
            PlayEffect(signal.EffectId, signal.Position);
        }

        private void PlayEffect(string effectId, Vector3 position)
        {
            if (string.IsNullOrEmpty(effectId))
                return;

            GameObject effectPrefab = GetEffectPrefab(effectId);
            if (!effectPrefab)
                return;

            Instantiate(effectPrefab, position, Quaternion.identity);
        }

        private GameObject GetEffectPrefab(string effectId)
        {
            return _effectPrefabs.GetValueOrDefault(effectId);
        }

        public void RegisterEffect(string effectId, GameObject prefab)
        {
            _effectPrefabs[effectId] = prefab;
        }
    }
}
