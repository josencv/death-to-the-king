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
    public class TestFromComponent : ZenjectIntegrationTestFixture
    {
        [Test]
        public void TestBasic()
        {
            var gameObject = Container.CreateEmptyGameObject("Foo");

            Container.BindInstance(gameObject, true).WithId("Foo");

            Container.Bind<Foo>().FromNewComponentOn(gameObject).AsSingle();
            Container.BindRootResolve<Foo>();

            Initialize();

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
        }

        [Test]
        public void TestBasicByMethod()
        {
            var gameObject = Container.CreateEmptyGameObject("Foo");

            Container.BindInstance(gameObject, true).WithId("Foo");

            Container.Bind<Foo>().FromNewComponentOn((context) => gameObject).AsSingle();
            Container.BindRootResolve<Foo>();

            Initialize();

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
        }

        [Test]
        public void TestTransient()
        {
            var gameObject = Container.CreateEmptyGameObject("Foo");

            Container.BindInstance(gameObject, true).WithId("Foo");

            Container.Bind<Foo>().FromNewComponentOn(gameObject).AsTransient();
            Container.Bind<IFoo>().To<Foo>().FromNewComponentOn(gameObject).AsTransient();

            Container.BindRootResolve(new[] {typeof(IFoo), typeof(Foo)});

            Initialize();

            FixtureUtil.AssertComponentCount<Foo>(Container, 2);
        }

        [Test]
        public void TestSingle()
        {
            var gameObject = Container.CreateEmptyGameObject("Foo");

            Container.BindInstance(gameObject, true).WithId("Foo");

            Container.Bind<Foo>().FromNewComponentOn(gameObject).AsSingle();
            Container.Bind<IFoo>().To<Foo>().FromNewComponentOn(gameObject).AsSingle();

            Container.BindRootResolve<IFoo>();
            Container.BindRootResolve<Foo>();

            Initialize();

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
        }

        [Test]
        public void TestCached1()
        {
            var gameObject = Container.CreateEmptyGameObject("Foo");

            Container.BindInstance(gameObject, true).WithId("Foo");

            Container.Bind<Foo>().FromNewComponentOn(gameObject).AsCached();
            Container.Bind<IFoo>().To<Foo>().FromNewComponentOn(gameObject).AsCached();

            Container.BindRootResolve<IFoo>();
            Container.BindRootResolve<Foo>();

            Initialize();

            FixtureUtil.AssertComponentCount<Foo>(Container, 2);
        }

        [Test]
        public void TestCached2()
        {
            var gameObject = Container.CreateEmptyGameObject("Foo");

            Container.BindInstance(gameObject, true).WithId("Foo");

            Container.Bind(typeof(IFoo), typeof(Foo)).To<Foo>().FromNewComponentOn(gameObject).AsCached();

            Container.BindRootResolve<IFoo>();
            Container.BindRootResolve<Foo>();

            Initialize();

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
        }

        [Test]
        public void TestCachedMultipleConcrete()
        {
            var gameObject = Container.CreateEmptyGameObject("Foo");

            Container.BindInstance(gameObject, true).WithId("Foo");

            Container.Bind(typeof(IFoo), typeof(IBar))
                .To(new List<Type>() { typeof(Foo), typeof(Bar) }).FromNewComponentOn(gameObject).AsCached();

            Container.BindRootResolve(new [] { typeof(IFoo), typeof(IBar) });

            Initialize();

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertComponentCount<Bar>(Container, 1);
        }

        [Test]
        public void TestSingleMultipleConcrete()
        {
            var gameObject = Container.CreateEmptyGameObject("Foo");

            Container.BindInstance(gameObject, true).WithId("Foo");

            Container.Bind(typeof(IFoo), typeof(IBar)).To(new List<Type>() { typeof(Foo), typeof(Bar) })
                .FromNewComponentOn(gameObject).AsSingle();
            Container.Bind<IFoo2>().To<Foo>().FromNewComponentOn(gameObject).AsSingle();

            Container.BindRootResolve(new [] { typeof(IFoo), typeof(IFoo2), typeof(IBar) });

            Initialize();

            FixtureUtil.AssertComponentCount<Foo>(Container, 1);
            FixtureUtil.AssertComponentCount<Bar>(Container, 1);
        }

        public interface IBar
        {
        }

        public interface IFoo2
        {
        }

        public interface IFoo
        {
        }

        public class Foo : MonoBehaviour, IFoo, IBar, IFoo2
        {
        }

        public class Bar : MonoBehaviour, IFoo, IBar, IFoo2
        {
        }
    }
}
