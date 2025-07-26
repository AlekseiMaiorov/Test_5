using System;
using UnityEditor;
using UnityEngine;

namespace Project.Game.Shapes
{
    public enum ShapeType
    {
        Default
    }

    [CreateAssetMenu(fileName = "New Shape Data", menuName = "Game/Shape Data")]
    public class ShapeData : ScriptableObject
    {
        [Header("Basic Info")]
        [SerializeField] private int _id;
        [SerializeField] private string _name;
        
        [Header("Behaviour")]
        [SerializeField] private ShapeType _shapeType;
        
        [Header("Visual")]
        [SerializeField] private Sprite _sprite;
        
        [Header("Game Values")]
        [SerializeField] private int _scoreValue = 1;
        [SerializeField] private int _healthPenalty = 1;

        [Header("Effects")]
        [SerializeField] private string _explosionEffectId = "explosion_default";
        [SerializeField] private string _matchEffectId = "match_default";
        [SerializeField] private string _matchSoundId = "match_sound";
        [SerializeField] private string _explosionSoundId = "explosion_sound";
        
        public int Id => _id;
        public string Name => _name;
        public Sprite Sprite => _sprite;
        public int ScoreValue => _scoreValue;
        public int HealthPenalty => _healthPenalty;
        public string ExplosionEffectId => _explosionEffectId;
        public string MatchEffectId => _matchEffectId;
        public string MatchSoundId => _matchSoundId;
        public string ExplosionSoundId => _explosionSoundId;
        public ShapeType ShapeType => _shapeType;
    }
}