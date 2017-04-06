using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using ModestTree;
using Assert=ModestTree.Assert;
using Zenject.Tests.Bindings.FromPrefab;

namespace Zenject.Tests.Bindings
{
    [TestFixture]
    public class TestFromPrefab : ZenjectIntegrationTestFixture
    {
        GameObject FooPrefab
        {
            get { return GetPrefab("Foo"); }
        }

        GameObject FooPrefab2
        {
            get { return GetPrefab("Foo2"); }
        }

        GameObject GorpPrefab
        {
            get { return GetPrefab("Gorp"); }
        }

        GameObject GorpAndQuxPrefab
        {
            get { return GetPrefab("GorpAndQux"); }
        }

        GameObject NorfPrefab
        {
            get { return GetPrefab("Norf"); }
        }

        GameObject JimAndBobPrefab
        {
            get { return GetPrefab("JimAndBob"); }
        }

        [Test]
        public void TestTransient()
        {
            Container.Bind<Foo>().FromComponentInNewPrefab(FooPrefab).AsTransient();
            Container.Bind<Foo>().FromComponentInNewPrefab(FooPrefab).AsTransient();

            Container.BindRootResolve<Foo>();

            Initialize();

            FixtureUtil.AssertComponentCount<Foo>(Container, 2);
        }

        [Test]
        public void TestSingle()
        {
            Container.Bind<IFoo>().To<Foo>().FromComponentInNewPrefab(FooPrefab).AsSingle().NonLazy();
            Container.Bind<Foo>().FromComponentInNewPrefab(FooPrefab).AsSingle().NonLazy();

            Initialize();

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
        }

        [Test]
        public void TestSingle2()
        {
            // For ToPrefab, the 'AsSingle' applies to the prefab and not the type, so this is valid
            Container.Bind<IFoo>().To<Foo>().FromComponentInNewPrefab(FooPrefab).AsSingle();
            Container.Bind<Foo>().FromComponentInNewPrefab(FooPrefab2).AsSingle();
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
            Container.Bind<Foo>().FromComponentInNewPrefab(FooPrefab).WithGameObjectName("Foo").AsSingle().NonLazy();
            Container.Bind<Bar>().FromComponentInNewPrefab(FooPrefab).WithGameObjectName("Foo").AsSingle().NonLazy();

            Initialize();

            FixtureUtil.AssertNumGameObjects(Container, 1);
            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertComponentCount<Bar>(Container, 1);
            FixtureUtil.AssertNumGameObjectsWithName(Container, "Foo", 1);
        }

        [Test]
        public void TestCached1()
        {
            Container.Bind(typeof(Foo), typeof(Bar)).FromComponentInNewPrefab(FooPrefab).WithGameObjectName("Foo").AsCached().NonLazy();

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
            Container.Bind(typeof(Gorp), typeof(Qux)).FromComponentInNewPrefab(GorpAndQuxPrefab).AsCached().NonLazy();

            Assert.Throws(() => Initialize());
        }

        [Test]
        public void TestWithArgumentsFail2()
        {
            Container.Bind(typeof(Gorp), typeof(Qux))
                .FromComponentInNewPrefab(GorpAndQuxPrefab).WithGameObjectName("Gorp").AsCached()
                .WithArguments(5, "test1").NonLazy();

            Assert.Throws(() => Initialize());
        }

        [Test]
        public void TestWithArgumentsSuccess()
        {
            Container.Bind<Gorp>().FromComponentInNewPrefab(GorpPrefab)
                .WithGameObjectName("Gorp").AsCached()
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
            Container.Bind<INorf>().FromComponentInNewPrefab(NorfPrefab).AsTransient().NonLazy();

            Initialize();

            FixtureUtil.AssertNumGameObjects(Container, 1);
            FixtureUtil.AssertComponentCount<INorf>(Container, 3);
            FixtureUtil.AssertResolveCount<INorf>(Container, 3);
        }

        [Test]
        public void TestAbstractBindingConcreteSearch()
        {
            // Should ignore the Norf2 component on it
            Container.Bind<INorf>().To<Norf>().FromComponentInNewPrefab(NorfPrefab).AsTransient().NonLazy();

            Initialize();

            FixtureUtil.AssertNumGameObjects(Container, 1);
            FixtureUtil.AssertResolveCount<INorf>(Container, 2);
        }

        [Test]
        public void TestCircularDependencies()
        {
            // Jim and Bob both depend on each other
            Container.Bind(typeof(Jim), typeof(Bob)).FromComponentInNewPrefab(JimAndBobPrefab).AsCached().NonLazy();
            Container.BindInterfacesTo<JimAndBobRunner>().AsSingle().NonLazy();

            Initialize();
        }

        GameObject GetPrefab(string name)
        {
            return FixtureUtil.GetPrefab("TestFromPrefab/{0}".Fmt(name));
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
