using Project.Game.Events;
using Zenject;

namespace Project.Game.Installers
{
    public class SignalsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            
            Container.DeclareSignal<ShapeSpawnedSignal>();
            Container.DeclareSignal<ShapeMatchedSignal>();
            Container.DeclareSignal<ShapeMismatchedSignal>();
            Container.DeclareSignal<ShapeReachedDeathZoneSignal>();
            Container.DeclareSignal<ShapeDestroyedByExplosionSignal>();
            Container.DeclareSignal<PlayerStatsChangedSignal>();
            Container.DeclareSignal<GameStateChangedSignal>();
            Container.DeclareSignal<StartSpawnShapesSignal>();
            Container.DeclareSignal<StopSpawnShapesSignal>();
            Container.DeclareSignal<RestartGameSignal>();
            Container.DeclareSignal<ShapeSpawnerStoppedSignal>();
        }
    }
}