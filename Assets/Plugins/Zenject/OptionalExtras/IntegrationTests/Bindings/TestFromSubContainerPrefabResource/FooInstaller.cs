using UnityEngine;
using System.Collections;
using Zenject;

#pragma warning disable 649

namespace Zenject.Tests.Bindings.FromSubContainerPrefabResource
{
    public class FooInstaller : MonoInstaller
    {
        [SerializeField]
        Bar _bar;

        public override void InstallBindings()
        {
            Container.BindInstance(_bar, true);
            Container.Bind<Gorp>().WithId("gorp").AsSingle();
        }
    }
}
