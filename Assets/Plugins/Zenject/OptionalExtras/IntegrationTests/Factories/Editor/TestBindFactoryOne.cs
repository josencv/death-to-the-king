using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using ModestTree;
using Assert=ModestTree.Assert;
using Zenject.Tests.Factories.BindFactoryOne;

namespace Zenject.Tests.Factories
{
    [TestFixture]
    public class TestBindFactoryOne : ZenjectIntegrationTestFixture
    {
        GameObject FooPrefab
        {
            get
            {
                return FixtureUtil.GetPrefab("TestBindFactoryOne/Foo");
            }
        }

        GameObject FooSubContainerPrefab
        {
            get
            {
                return FixtureUtil.GetPrefab("TestBindFactoryOne/FooSubContainer");
            }
        }

        [Test]
        public void TestFromNewComponentOn()
        {
            var go = new GameObject();

            Container.BindFactory<string, Foo, Foo.Factory>().FromNewComponentOn(go);

            Initialize();

            var factory = Container.Resolve<Foo.Factory>();

            Assert.IsNull(go.GetComponent<Foo>());
            var foo = factory.Create("asdf");
            Assert.IsEqual(foo.Value, "asdf");
            Assert.IsNotNull(go.GetComponent<Foo>());
            Assert.IsEqual(go.GetComponent<Foo>(), foo);

            var foo2 = factory.Create("zxcv");

            Assert.IsNotEqual(foo2, foo);

            var allFoos = go.GetComponents<Foo>();
            Assert.IsEqual(allFoos.Length, 2);
            Assert.IsEqual(allFoos[0], foo);
            Assert.IsEqual(allFoos[1], foo2);
        }

        [Test]
        public void TestFromNewScriptableObjectResource()
        {
            Container.BindFactory<string, Bar, Bar.Factory>()
                .FromNewScriptableObjectResource("TestBindFactoryOne/Bar");

            Initialize();

            var factory = Container.Resolve<Bar.Factory>();
            var bar = factory.Create("asdf");
            Assert.IsNotNull(bar);
            Assert.IsEqual(bar.Value, "asdf");
            Assert.IsNotEqual(bar, factory.Create("zxcv"));
        }

        [Test]
        public void TestFromNewComponentOnNewGameObjectSelf()
        {
            Container.BindFactory<string, Foo, Foo.Factory>().FromNewComponentOnNewGameObject();

            AddFactoryUser<Foo, Foo.Factory>();

            Initialize();

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
        }

        [Test]
        public void TestFromNewComponentOnNewGameObjectConcrete()
        {
            Container.BindFactory<string, IFoo, IFooFactory>().To<Foo>().FromNewComponentOnNewGameObject();

            AddFactoryUser<IFoo, IFooFactory>();

            Initialize();

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
        }

        [Test]
        public void TestFromNewComponentOnSelf()
        {
            var gameObject = Container.CreateEmptyGameObject("foo");

            Container.BindFactory<string, Foo, Foo.Factory>().FromNewComponentOn(gameObject);

            AddFactoryUser<Foo, Foo.Factory>();

            Initialize();

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
        }

        [Test]
        public void TestFromNewComponentOnConcrete()
        {
            var gameObject = Container.CreateEmptyGameObject("foo");

            Container.BindFactory<string, IFoo, IFooFactory>().To<Foo>().FromNewComponentOn(gameObject);

            AddFactoryUser<IFoo, IFooFactory>();

            Initialize();

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
        }

        [Test]
        public void TestFromComponentInNewPrefabSelf()
        {
            Container.BindFactory<string, Foo, Foo.Factory>().FromComponentInNewPrefab(FooPrefab).WithGameObjectName("asdf");

            AddFactoryUser<Foo, Foo.Factory>();

            Initialize();

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
            FixtureUtil.AssertNumGameObjectsWithName(Container, "asdf", 1);
        }

        [Test]
        public void TestFromComponentInNewPrefabConcrete()
        {
            Container.BindFactory<string, IFoo, IFooFactory>().To<Foo>()
                .FromComponentInNewPrefab(FooPrefab).WithGameObjectName("asdf");

            AddFactoryUser<IFoo, IFooFactory>();

            Initialize();

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
            FixtureUtil.AssertNumGameObjectsWithName(Container, "asdf", 1);
        }

        [Test]
        public void TestFromComponentInNewPrefabResourceSelf()
        {
            Container.BindFactory<string, Foo, Foo.Factory>().FromComponentInNewPrefabResource("TestBindFactoryOne/Foo").WithGameObjectName("asdf");

            AddFactoryUser<Foo, Foo.Factory>();

            Initialize();

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
            FixtureUtil.AssertNumGameObjectsWithName(Container, "asdf", 1);
        }

        [Test]
        public void TestFromComponentInNewPrefabResourceConcrete()
        {
            Container.BindFactory<string, IFoo, IFooFactory>().To<Foo>()
                .FromComponentInNewPrefabResource("TestBindFactoryOne/Foo").WithGameObjectName("asdf");

            AddFactoryUser<IFoo, IFooFactory>();

            Initialize();

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
            FixtureUtil.AssertNumGameObjectsWithName(Container, "asdf", 1);
        }

        [Test]
        public void TestFromSubContainerResolveByNewPrefabSelf()
        {
            Container.BindFactory<string, Foo, Foo.Factory>()
                .FromSubContainerResolve().ByNewPrefab<FooInstaller>(FooSubContainerPrefab);

            AddFactoryUser<Foo, Foo.Factory>();

            Initialize();

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
        }

        [Test]
        public void TestFromSubContainerResolveByNewPrefabConcrete()
        {
            Container.BindFactory<string, IFoo, IFooFactory>()
                .To<Foo>().FromSubContainerResolve().ByNewPrefab<FooInstaller>(FooSubContainerPrefab);

            AddFactoryUser<IFoo, IFooFactory>();

            Initialize();

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
        }

        [Test]
        public void TestFromSubContainerResolveByNewPrefabResourceSelf()
        {
            Container.BindFactory<string, Foo, Foo.Factory>()
                .FromSubContainerResolve().ByNewPrefabResource<FooInstaller>("TestBindFactoryOne/FooSubContainer");

            AddFactoryUser<Foo, Foo.Factory>();

            Initialize();

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
        }

        [Test]
        public void TestFromSubContainerResolveByNewPrefabResourceConcrete()
        {
            Container.BindFactory<string, IFoo, IFooFactory>()
                .To<Foo>().FromSubContainerResolve().ByNewPrefabResource<FooInstaller>("TestBindFactoryOne/FooSubContainer");

            AddFactoryUser<IFoo, IFooFactory>();

            Initialize();

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertNumGameObjects(Container, 1);
        }

        void AddFactoryUser<TValue, TFactory>()
            where TValue : IFoo
            where TFactory : Factory<string, TValue>
        {
            Container.Bind<IInitializable>()
                .To<FooFactoryTester<TValue, TFactory>>().AsSingle();

            Container.BindExecutionOrder<FooFactoryTester<TValue, TFactory>>(-100);
        }

        public class FooFactoryTester<TValue, TFactory> : IInitializable
            where TFactory : Factory<string, TValue>
            where TValue : IFoo
        {
            readonly TFactory _factory;

            public FooFactoryTester(TFactory factory)
            {
                _factory = factory;
            }

            public void Initialize()
            {
                Assert.IsEqual(_factory.Create("asdf").Value, "asdf");

                Log.Info("Factory created foo successfully");
            }
        }
    }
}
