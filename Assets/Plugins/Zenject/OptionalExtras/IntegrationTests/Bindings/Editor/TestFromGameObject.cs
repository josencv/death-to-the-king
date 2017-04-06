using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using ModestTree;
using Assert=ModestTree.Assert;

namespace Zenject.Tests.Bindings
{
    [TestFixture]
    public class TestFromGameObject : ZenjectIntegrationTestFixture
    {
        const string GameObjName = "TestObj";

        [Test]
        public void TestBasic()
        {
            Container.Bind<Foo>().FromNewComponentOnNewGameObject().WithGameObjectName(GameObjName).AsSingle();
            Container.BindRootResolve<Foo>();

            Initialize();

            FixtureUtil.AssertNumGameObjects(Container, 1);
            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
        }

        [Test]
        public void TestSingle()
        {
            Container.Bind<Foo>().FromNewComponentOnNewGameObject().WithGameObjectName(GameObjName).AsSingle();
            Container.Bind<IFoo>().To<Foo>().FromNewComponentOnNewGameObject().WithGameObjectName(GameObjName).AsSingle();

            Container.BindRootResolve<IFoo>();
            Container.BindRootResolve<Foo>();

            Initialize();

            FixtureUtil.AssertNumGameObjects(Container, 1);
            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
        }

        [Test]
        public void TestTransient()
        {
            Container.Bind<Foo>().FromNewComponentOnNewGameObject().WithGameObjectName(GameObjName).AsTransient();
            Container.Bind<IFoo>().To<Foo>().FromNewComponentOnNewGameObject().WithGameObjectName(GameObjName).AsTransient();

            Container.BindRootResolve<IFoo>();
            Container.BindRootResolve<Foo>();

            Initialize();

            FixtureUtil.AssertNumGameObjects(Container, 2);
            FixtureUtil.AssertComponentCount<Foo>(Container, 2);
        }

        [Test]
        public void TestCached1()
        {
            Container.Bind<Foo>().FromNewComponentOnNewGameObject().WithGameObjectName(GameObjName).AsCached();
            Container.Bind<IFoo>().To<Foo>().FromNewComponentOnNewGameObject().WithGameObjectName(GameObjName).AsCached();

            Container.BindRootResolve<IFoo>();
            Container.BindRootResolve<Foo>();

            Initialize();

            FixtureUtil.AssertNumGameObjects(Container, 2);
            FixtureUtil.AssertComponentCount<Foo>(Container, 2);
        }

        [Test]
        public void TestCached2()
        {
            Container.Bind(typeof(Foo), typeof(IFoo)).To<Foo>().FromNewComponentOnNewGameObject().WithGameObjectName(GameObjName).AsCached();

            Container.BindRootResolve<IFoo>();
            Container.BindRootResolve<Foo>();

            Initialize();

            FixtureUtil.AssertNumGameObjects(Container, 1);
            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
        }

        [Test]
        public void TestMultipleConcreteTransient1()
        {
            Container.Bind<IFoo>().To(typeof(Foo), typeof(Bar)).FromNewComponentOnNewGameObject()
                .WithGameObjectName(GameObjName).AsTransient();

            Container.BindRootResolve<IFoo>();

            Initialize();

            FixtureUtil.AssertNumGameObjects(Container, 2);
            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertComponentCount<Bar>(Container, 1);
        }

        [Test]
        public void TestMultipleConcreteTransient2()
        {
            Container.Bind(typeof(IFoo), typeof(IBar)).To(new List<Type>() {typeof(Foo), typeof(Bar)}).FromNewComponentOnNewGameObject()
                .WithGameObjectName(GameObjName).AsTransient();

            Container.BindRootResolve<IFoo>();
            Container.BindRootResolve<IBar>();

            Initialize();

            FixtureUtil.AssertNumGameObjects(Container, 4);
            FixtureUtil.AssertComponentCount<Foo>(Container, 2);
            FixtureUtil.AssertComponentCount<Bar>(Container, 2);
        }

        [Test]
        public void TestMultipleConcreteCached()
        {
            Container.Bind(typeof(IFoo), typeof(IBar)).To(new List<Type>() {typeof(Foo), typeof(Bar)}).FromNewComponentOnNewGameObject()
                .WithGameObjectName(GameObjName).AsCached();

            Container.BindRootResolve<IFoo>();
            Container.BindRootResolve<IBar>();

            Initialize();

            FixtureUtil.AssertNumGameObjects(Container, 2);
            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertComponentCount<Bar>(Container, 1);
        }

        [Test]
        public void TestMultipleConcreteSingle()
        {
            Container.Bind(typeof(IFoo), typeof(IBar)).To(new List<Type>() {typeof(Foo), typeof(Bar)}).FromNewComponentOnNewGameObject()
                .WithGameObjectName(GameObjName).AsSingle();

            Container.BindRootResolve<IFoo>();
            Container.BindRootResolve<IBar>();

            Initialize();

            FixtureUtil.AssertNumGameObjects(Container, 2);
        }


        [Test]
        public void TestUnderTransformGroup()
        {
            Container.Bind<Foo>().FromNewComponentOnNewGameObject()
                .WithGameObjectName(GameObjName).UnderTransformGroup("Foo").AsSingle();
            Container.BindRootResolve<Foo>();

            Initialize();

            var root = Container.Resolve<Context>().transform;
            var child1 = root.GetChild(0);

            Assert.IsEqual(child1.name, "Foo");

            var child2 = child1.GetChild(0);

            Assert.IsNotNull(child2.GetComponent<Foo>());
        }

        [Test]
        public void TestUnderTransform()
        {
            var tempGameObject = new GameObject("Foo");

            Container.Bind<Foo>().FromNewComponentOnNewGameObject()
                .WithGameObjectName(GameObjName).UnderTransform(tempGameObject.transform).AsSingle();
            Container.BindRootResolve<Foo>();

            Initialize();

            Assert.IsNotNull(tempGameObject.transform.GetChild(0).GetComponent<Foo>());
        }

        [Test]
        public void TestUnderTransformGetter()
        {
            var tempGameObject = new GameObject("Foo");

            Container.Bind<Foo>().FromNewComponentOnNewGameObject()
                .WithGameObjectName(GameObjName).UnderTransform((context) => tempGameObject.transform).AsSingle();
            Container.BindRootResolve<Foo>();

            Initialize();

            Assert.IsNotNull(tempGameObject.transform.GetChild(0).GetComponent<Foo>());
        }

        public interface IBar
        {
        }

        public interface IFoo
        {
        }

        public class Foo : MonoBehaviour, IFoo, IBar
        {
        }

        public class Bar : MonoBehaviour, IFoo, IBar
        {
        }
    }
}
