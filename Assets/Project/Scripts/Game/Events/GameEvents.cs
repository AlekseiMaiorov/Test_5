using Project.Game.Core;
using Project.Game.Shapes;
using UnityEngine;

namespace Project.Game.Events
{
    public struct ShapeSpawnedSignal
    {
        public DefaultShape Shape;
        public Vector3 Position;
        public int Lane;
    }

    public struct ShapeMatchedSignal
    {
        public DefaultShape Shape;
        public SortingSlot Slot;
        public Vector3 Position;
        public int ScoreGained;
        public string EffectId;
        public string SoundId;
    }

    public struct ShapeMismatchedSignal
    {
        public DefaultShape Shape;
        public SortingSlot Slot; 
        public Vector3 Position;
        public int HealthLost;
        public string EffectId;
        public string SoundId;
    }

    public struct ShapeReachedDeathZoneSignal
    {
        public DefaultShape Shape;
        public Vector3 Position;
        public int HealthLost;
        public string EffectId;
        public string SoundId;
    }

    public struct ShapeDestroyedByExplosionSignal
    {
        public DefaultShape Shape;
        public Vector3 ExplosionCenter;
    }

    public struct PlayerStatsChangedSignal
    {
        public int Health;
        public int Score;
        public int HealthDelta;
        public int ScoreDelta;
    }

    public struct GameStateChangedSignal
    {
        public GameState NewState;
        public GameState PreviousState;
    }

    public struct StartSpawnShapesSignal
    {
    }

    public struct StopSpawnShapesSignal
    {
    }
    
    public struct ShapeSpawnerStoppedSignal
    {
    }

    public struct RestartGameSignal
    {
    }
}