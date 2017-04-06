using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using ModestTree;
using Assert=ModestTree.Assert;
using Zenject.Tests.Bindings.FromPrefabResource;

namespace Zenject.Tests.Bindings
{
    [TestFixture]
    public class TestFromPrefabResource : ZenjectIntegrationTestFixture
    {
        const string PathPrefix = "TestFromPrefabResource/";

        [Test]
        public void TestTransientError()
        {
            // Validation should detect that it doesn't exist
            Container.Bind<Foo>().FromComponentInNewPrefabResource(PathPrefix + "asdfasdfas").AsTransient().NonLazy();

            Assert.Throws(() => Initialize());
        }

        [Test]
        public void TestTransient()
        {
            Container.Bind<Foo>().FromComponentInNewPrefabResource(PathPrefix + "Foo").AsTransient();
            Container.Bind<Foo>().FromComponentInNewPrefabResource(PathPrefix + "Foo").AsTransient();

            Container.BindRootResolve<Foo>();

            Initialize();

            FixtureUtil.AssertComponentCount<Foo>(Container, 2);
        }

        [Test]
        public void TestSingle()
        {
            Container.Bind<IFoo>().To<Foo>().FromComponentInNewPrefabResource(PathPrefix + "Foo").AsSingle().NonLazy();
            Container.Bind<Foo>().FromComponentInNewPrefabResource(PathPrefix + "Foo").AsSingle().NonLazy();

            Initialize();

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
        }

        [Test]
        public void TestSingle2()
        {
            // For ToPrefab, the 'AsSingle' applies to the prefab and not the type, so this is valid
            Container.Bind<IFoo>().To<Foo>().FromComponentInNewPrefabResource(PathPrefix + "Foo").AsSingle();
            Container.Bind<Foo>().FromComponentInNewPrefabResource(PathPrefix + "Foo2").AsSingle();
            Container.Bind<Foo>().FromMethod(ctx => ctx.Container.CreateEmptyGameObject("Foo").AddComponent<Foo>());

            Container.BindRootResolve<Foo>();
            Container.BindRootResolve<IFoo>();

            Initialize();

            FixtureUtil.AssertComponentCount<Foo>(Container, 3);
            FixtureUtil.AssertNumGameObjects(Container, 3);
        }

        [Test]
        public void TestSingleIdentifiers()
        {
            Container.Bind<Foo>().FromComponentInNewPrefabResource(PathPrefix + "Foo").WithGameObjectName("Foo").AsSingle().NonLazy();
            Container.Bind<Bar>().FromComponentInNewPrefabResource(PathPrefix + "Foo").WithGameObjectName("Foo").AsSingle().NonLazy();

            Initialize();

            FixtureUtil.AssertNumGameObjects(Container, 1);
            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertComponentCount<Bar>(Container, 1);
            FixtureUtil.AssertNumGameObjectsWithName(Container, "Foo", 1);
        }

        [Test]
        public void TestCached1()
        {
            Container.Bind(typeof(Foo), typeof(Bar)).FromComponentInNewPrefabResource(PathPrefix + "Foo")
                .WithGameObjectName("Foo").AsCached().NonLazy();

            Initialize();

            FixtureUtil.AssertNumGameObjects(Container, 1);
            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertComponentCount<Bar>(Container, 1);
            FixtureUtil.AssertNumGameObjectsWithName(Container, "Foo", 1);
        }

        [Test]
        public void TestWithArgumentsFail()
        {
            // They have required arguments
            Container.Bind(typeof(Gorp), typeof(Qux)).FromComponentInNewPrefabResource(PathPrefix + "GorpAndQux").AsCached().NonLazy();

            Assert.Throws(() => Initialize());
        }

        [Test]
        public void TestWithArguments()
        {
            Container.Bind(typeof(Gorp))
                .FromComponentInNewPrefabResource(PathPrefix + "Gorp").WithGameObjectName("Gorp").AsCached()
                .WithArguments("test1").NonLazy();

            Initialize();

            FixtureUtil.AssertNumGameObjects(Container, 1);
            FixtureUtil.AssertComponentCount<Gorp>(Container, 1);
            FixtureUtil.AssertNumGameObjectsWithName(Container, "Gorp", 1);
        }

        [Test]
        public void TestWithAbstractSearch()
        {
            // There are three components that implement INorf on this prefab
            // and so this should result in a list of 3 INorf's
            Container.Bind<INorf>().FromComponentInNewPrefabResource(PathPrefix + "Norf").AsTransient().NonLazy();

            Initialize();

            FixtureUtil.AssertNumGameObjects(Container, 1);
            FixtureUtil.AssertComponentCount<INorf>(Container, 3);
            FixtureUtil.AssertResolveCount<INorf>(Container, 3);
        }

        [Test]
        public void TestAbstractBindingConcreteSearch()
        {
            // Should ignore the Norf2 component on it
            Container.Bind<INorf>().To<Norf>().FromComponentInNewPrefabResource(PathPrefix + "Norf").AsTransient().NonLazy();

            Initialize();

            FixtureUtil.AssertNumGameObjects(Container, 1);
            FixtureUtil.AssertResolveCount<INorf>(Container, 2);
        }

        [Test]
        public void TestCircularDependencies()
        {
            // Jim and Bob both depend on each other
            Container.Bind(typeof(Jim), typeof(Bob)).FromComponentInNewPrefabResource(PathPrefix + "JimAndBob").AsCached().NonLazy();

            Container.BindInterfacesTo<JimAndBobRunner>().AsSingle().NonLazy();

            Initialize();
        }

        public class JimAndBobRunner : IInitializable
        {
            readonly Bob _bob;
            readonly Jim _jim;

            public JimAndBobRunner(Jim jim, Bob bob)
            {
                _bob = bob;
                _jim = jim;
            }

            public void Initialize()
            {
                Assert.IsNotNull(_jim.Bob);
                Assert.IsNotNull(_bob.Jim);

                Log.Info("Jim and bob successfully got the other reference");
            }
        }
    }
}
