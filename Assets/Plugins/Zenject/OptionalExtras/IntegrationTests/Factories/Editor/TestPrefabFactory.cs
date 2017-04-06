using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using ModestTree;
using Assert=ModestTree.Assert;
using Zenject.Tests.Factories.PrefabFactory;

namespace Zenject.Tests.Factories
{
    [TestFixture]
    public class TestPrefabFactory : ZenjectIntegrationTestFixture
    {
        GameObject FooPrefab
        {
            get
            {
                return FixtureUtil.GetPrefab("TestPrefabFactory/Foo");
            }
        }

        [Test]
        public void Test1()
        {
            Container.Bind<FooFactory>().ToSelf().AsSingle();
            Container.Bind<IInitializable>().To<Runner>().AsSingle().WithArguments(FooPrefab);

            Initialize();
        }

        public class FooFactory : PrefabFactory<Foo>
        {
        }

        public class Runner : IInitializable
        {
            readonly GameObject _prefab;
            readonly FooFactory _fooFactory;

            public Runner(
                FooFactory fooFactory,
                GameObject prefab)
            {
                _prefab = prefab;
                _fooFactory = fooFactory;
            }

            public void Initialize()
            {
                var foo = _fooFactory.Create(_prefab);

                Assert.That(foo.WasInitialized);
            }
        }
    }
}
