using Assets.Code.Components.Containers;
using Assets.Code.Components.Controller;
using Assets.Code.Components.Movement;
using Assets.Code.Infrastructure.Input;
using Assets.Code.Infrastructure.Sound;
using System;
using UnityEngine;
using Zenject;

namespace Assets.Code.Configuration
{
    public class CoreInstaller : MonoInstaller  
    {
        [Inject]
        Settings settings = null;

        public override void InstallBindings()
        {
            // General

            Container.Bind<GameManager>().AsSingle();
            Container.Bind<IInputManager>().To<InputManager>().AsSingle();
            Container.Bind<IGameInput>().To<KeyboardGameInput>().AsTransient();
            Container.Bind<ISoundManager>().To<SoundManager2D>().AsSingle();

            // Components

            Container.Bind<IEntityController>().FromComponentSibling();
            Container.Bind<IMovable>().FromComponentSibling();
            Container.Bind<Rigidbody2D>().FromComponentSibling();
            Container.Bind<SpriteRenderer>().FromComponentSibling();

            // Containers

            Container.BindFactory<Character, Factory<Character>>()
                .FromComponentInNewPrefab(settings.PlayerPrefab)
                .WithGameObjectName("Character")
                .UnderTransformGroup("Characters");
        }

        [Serializable]
        public class Settings
        {
            public GameObject PlayerPrefab;
        }
    }
}
