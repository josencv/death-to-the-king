using Assets.Code.Infrastructure.Controller;
using Assets.Code.Infrastructure.Sound;
using Zenject;

namespace Assets.Code.Configuration
{
    public class CoreInstaller : MonoInstaller  
    {
        public override void InstallBindings()
        {
            Container.Bind<IInputManager>().To<InputManager>().AsSingle();
            Container.Bind<IGameInput>().To<GameInput>().AsTransient();
            Container.Bind<ISoundManager>().To<SoundManager2D>().AsSingle();
            Container.Bind<ISoundManager>().To<SoundManager2D>().AsSingle();
        }
    }
}
