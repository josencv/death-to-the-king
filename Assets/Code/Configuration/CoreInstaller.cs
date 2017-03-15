using Assets.Code.Infrastructure.Controller;
using Assets.Code.Infrastructure.Sound;
using Zenject;

namespace Assets.Code.Configuration
{
    public class CoreInstaller : MonoInstaller  
    {
        public override void InstallBindings()
        {
            Container.Bind<GameManager>().AsSingle();
            Container.Bind<IInputManager>().FromComponentInNewPrefabResource("Prefabs/InputManager").AsSingle();
            Container.Bind<IGameInput>().To<KeyboardGameInput>().AsTransient();
            Container.Bind<ISoundManager>().FromComponentInNewPrefabResource("Prefabs/SoundManager").AsSingle();
        }
    }
}
