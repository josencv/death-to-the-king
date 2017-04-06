using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using ModestTree;
using Assert=ModestTree.Assert;
using Zenject.Tests.Factories.BindFactoryFive;

namespace Zenject.Tests.Factories
{
    [TestFixture]
    public class TestBindFactoryFive : ZenjectIntegrationTestFixture
    {
        GameObject FooPrefab
        {
            get
            {
                return FixtureUtil.GetPrefab("TestBindFactoryFive/Foo");
            }
        }

        GameObject FooSubContainerPrefab
        {
            get
            {
                return FixtureUtil.GetPrefab("TestBindFactoryFive/FooSubContainer");
            }
        }

        [Test]
        public void TestToGameObjectSelf()
        {
            Container.BindFactory<double, int, float, string, char, Foo, Foo.Factory>().FromNewComponentOnNewGameObject();

            AddFactoryUser<Foo, Foo.Factory>();

            Initialize();

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
        }

        [Test]
        public void TestToGameObjectConcrete()
        {
            Container.BindFactory<double, int, float, string, char, IFoo, IFooFactory>().To<Foo>().FromNewComponentOnNewGameObject();

            AddFactoryUser<IFoo, IFooFactory>();

            Initialize();

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
        }

        [Test]
        public void TestToMonoBehaviourSelf()
        {
            var gameObject = Container.CreateEmptyGameObject("foo");

            Container.BindFactory<double, int, float, string, char, Foo, Foo.Factory>().FromNewComponentOn(gameObject);

            AddFactoryUser<Foo, Foo.Factory>();

            Initialize();

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
        }

        [Test]
        public void TestToMonoBehaviourConcrete()
        {
            var gameObject = Container.CreateEmptyGameObject("foo");

            Container.BindFactory<double, int, float, string, char, IFoo, IFooFactory>().To<Foo>().FromNewComponentOn(gameObject);

            AddFactoryUser<IFoo, IFooFactory>();

            Initialize();

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
        }

        [Test]
        public void TestToPrefabSelf()
        {
            Container.BindFactory<double, int, float, string, char, Foo, Foo.Factory>().FromComponentInNewPrefab(FooPrefab).WithGameObjectName("asdf");

            AddFactoryUser<Foo, Foo.Factory>();

            Initialize();

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
            FixtureUtil.AssertNumGameObjectsWithName(Container, "asdf", 1);
        }

        [Test]
        public void TestToPrefabConcrete()
        {
            Container.BindFactory<double, int, float, string, char, IFoo, IFooFactory>().To<Foo>().FromComponentInNewPrefab(FooPrefab).WithGameObjectName("asdf");

            AddFactoryUser<IFoo, IFooFactory>();

            Initialize();

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
            FixtureUtil.AssertNumGameObjectsWithName(Container, "asdf", 1);
        }

        [Test]
        public void TestToPrefabResourceSelf()
        {
            Container.BindFactory<double, int, float, string, char, Foo, Foo.Factory>().FromComponentInNewPrefabResource("TestBindFactoryFive/Foo").WithGameObjectName("asdf");

            AddFactoryUser<Foo, Foo.Factory>();

            Initialize();

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
            FixtureUtil.AssertNumGameObjectsWithName(Container, "asdf", 1);
        }

        [Test]
        public void TestToPrefabResourceConcrete()
        {
            Container.BindFactory<double, int, float, string, char, IFoo, IFooFactory>()
                .To<Foo>().FromComponentInNewPrefabResource("TestBindFactoryFive/Foo").WithGameObjectName("asdf");

            AddFactoryUser<IFoo, IFooFactory>();

            Initialize();

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
            FixtureUtil.AssertNumGameObjectsWithName(Container, "asdf", 1);
        }

        [Test]
        public void TestToSubContainerPrefabSelf()
        {
            Container.BindFactory<double, int, float, string, char, Foo, Foo.Factory>()
                .FromSubContainerResolve().ByNewPrefab<FooInstaller>(FooSubContainerPrefab);

            AddFactoryUser<Foo, Foo.Factory>();

            Initialize();

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
        }

        [Test]
        public void TestToSubContainerPrefabConcrete()
        {
            Container.BindFactory<double, int, float, string, char, IFoo, IFooFactory>()
                .To<Foo>().FromSubContainerResolve().ByNewPrefab<FooInstaller>(FooSubContainerPrefab);

            AddFactoryUser<IFoo, IFooFactory>();

            Initialize();

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
        }

        [Test]
        public void TestToSubContainerPrefabResourceSelf()
        {
            Container.BindFactory<double, int, float, string, char, Foo, Foo.Factory>().FromSubContainerResolve().ByNewPrefabResource<FooInstaller>("TestBindFactoryFive/FooSubContainer");

            AddFactoryUser<Foo, Foo.Factory>();

            Initialize();

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
        }

        [Test]
        public void TestToSubContainerPrefabResourceConcrete()
        {
            Container.BindFactory<double, int, float, string, char, IFoo, IFooFactory>()
                .To<Foo>().FromSubContainerResolve().ByNewPrefabResource<FooInstaller>("TestBindFactoryFive/FooSubContainer");

            AddFactoryUser<IFoo, IFooFactory>();

            Initialize();

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
        }

        void AddFactoryUser<TValue, TFactory>()
            where TValue : IFoo
            where TFactory : Factory<double, int, float, string, char, TValue>
        {
            Container.Bind<IInitializable>()
                .To<FooFactoryTester<TValue, TFactory>>().AsSingle();

            Container.BindExecutionOrder<FooFactoryTester<TValue, TFactory>>(-100);
        }

        public class FooFactoryTester<TValue, TFactory> : IInitializable
            where TFactory : Factory<double, int, float, string, char, TValue>
            where TValue : IFoo
        {
            readonly TFactory _factory;

            public FooFactoryTester(TFactory factory)
            {
                _factory = factory;
            }

            public void Initialize()
            {
                Assert.IsEqual(_factory.Create(0.15, 0, 2.4f, "zxcv", 'z').Value, "zxcv");

                Log.Info("Factory created foo successfully");
            }
        }
    }
}
