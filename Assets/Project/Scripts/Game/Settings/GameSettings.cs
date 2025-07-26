using UnityEngine;

namespace Project.Game.Settings
{
    [CreateAssetMenu(fileName = "SpawnSettings", menuName = "Game/Spawn Settings")]
    public class GameSettings : ScriptableObject
    {
        [Header("Shape Count")]
        [SerializeField] private Vector2 _shapesToSpawnRange = new Vector2(10f, 20f);

        [Header("Spawn Timing")]
        [SerializeField] private Vector2 _spawnTimeoutRange = new Vector2(1f, 3f);

        [Header("Shape Movement")]
        [SerializeField] private Vector2 _shapeSpeedRange = new Vector2(1f, 5f);

        [Header("Player Health")]
        [SerializeField] private int _playerHealth = 3;
        
        [Header("Pool Settings")]
        [SerializeField] private int _shapePoolSize = 20;

        public Vector2 ShapesToSpawnRange => _shapesToSpawnRange;
        public Vector2 SpawnTimeoutRange => _spawnTimeoutRange;
        public Vector2 ShapeSpeedRange => _shapeSpeedRange;
        public int PlayerHealth => _playerHealth;
        public int ShapePoolSize => _shapePoolSize;
    }
}