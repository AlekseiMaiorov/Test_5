using Project.Game.Factories;
using Project.Game.Pools;
using Project.Game.Providers;
using Project.Game.Settings;
using Project.Game.Systems;
using Project.Game.UI;
using Project.Game.Core;
using Project.Game.Interfaces;
using Project.Game.Spawners;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Project.Game.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [FormerlySerializedAs("_spawnSettings")]
        [Header("Settings")]
        [SerializeField] private GameSettings _gameSettings;

        [Header("Prefabs")]
        [SerializeField] private GameObject _shapePrefab;

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<VictoryHandler>().AsSingle();
            
            Container.Bind<GameSettings>()
                     .FromInstance(_gameSettings)
                     .AsSingle();
            
            BindShapePool();
            
            Container.Bind<IShapeFactory>()
                     .To<ShapeFactory>()
                     .AsSingle();
            
            Container.Bind<ISortingSlotsProvider>()
                     .To<SortingSlotsProvider>()
                     .FromComponentInHierarchy()
                     .AsSingle();
            
            Container.Bind<GameController>()
                     .FromComponentInHierarchy()
                     .AsSingle();

            
            Container.Bind<ScoreManager>()
                     .FromComponentInHierarchy()
                     .AsSingle();

            Container.Bind<HealthManager>()
                     .FromComponentInHierarchy()
                     .AsSingle();

            Container.Bind<EffectsManager>()
                     .FromComponentInHierarchy()
                     .AsSingle();

            Container.Bind<SoundManager>()
                     .FromComponentInHierarchy()
                     .AsSingle();

            Container.Bind<ShapeLifecycleManager>()
                     .FromComponentInHierarchy()
                     .AsSingle();

            
            Container.Bind<ShapeSpawner>()
                     .FromComponentInHierarchy()
                     .AsSingle();

            
            Container.Bind<GameUI>()
                     .FromComponentInHierarchy()
                     .AsSingle();

            Container.Bind<GameOverPopup>()
                     .FromComponentInHierarchy()
                     .AsSingle();

            Container.Bind<VictoryPopup>()
                     .FromComponentInHierarchy()
                     .AsSingle();
        }

        private void BindShapePool()
        {
            Container.BindMemoryPool<ShapePool, ShapePool.Pool>()
                     .WithInitialSize(_gameSettings.ShapePoolSize)
                     .FromComponentInNewPrefab(_shapePrefab)
                     .UnderTransformGroup("ShapePool");
        }
    }
}