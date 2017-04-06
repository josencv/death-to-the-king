using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using ModestTree;
using Assert=ModestTree.Assert;
using Zenject.Tests.Bindings.FromNewScriptableObjectResource;

namespace Zenject.Tests.Bindings
{
    [TestFixture]
    public class TestFromNewScriptableObjectResource : ZenjectIntegrationTestFixture
    {
        const string PathPrefix = "TestFromNewScriptableObjectResource/";

        [Test]
        public void TestTransientError()
        {
            // Validation should detect that it doesn't exist
            Container.Bind<Foo>().FromNewScriptableObjectResource(PathPrefix + "asdfasdfas").AsTransient().NonLazy();

            Assert.Throws(() => Initialize());
        }

        [Test]
        public void TestTransient()
        {
            Foo.InstanceCount = 0;
            Container.Bind<Foo>().FromNewScriptableObjectResource(PathPrefix + "Foo").AsTransient();

            Initialize();

            var foo = Container.Resolve<Foo>();
            Assert.That(foo.WasInjected);

            Assert.IsEqual(Foo.InstanceCount, 1);

            var foo2 = Container.Resolve<Foo>();
            Assert.IsNotEqual(foo, foo2);
            Assert.IsEqual(Foo.InstanceCount, 2);
        }

        [Test]
        public void TestSingle()
        {
            Foo.InstanceCount = 0;

            Container.Bind<IFoo>().To<Foo>().FromNewScriptableObjectResource(PathPrefix + "Foo").AsSingle();
            Container.Bind<Foo>().FromNewScriptableObjectResource(PathPrefix + "Foo").AsSingle();

            Initialize();

            Container.Resolve<IFoo>();
            Assert.IsEqual(Foo.InstanceCount, 1);
        }

        [Test]
        public void TestAbstractBinding()
        {
            Foo.InstanceCount = 0;

            Container.Bind<IFoo>().To<Foo>()
                .FromNewScriptableObjectResource(PathPrefix + "Foo").AsSingle().NonLazy();

            Initialize();

            Container.Resolve<IFoo>();
            Assert.IsEqual(Foo.InstanceCount, 1);
        }

        [Test]
        public void TestWithArgumentsFail()
        {
            Container.Bind<Bob>()
                .FromNewScriptableObjectResource(PathPrefix + "Bob").AsCached().NonLazy();

            Assert.Throws(() => Initialize());
        }

        [Test]
        public void TestWithArguments()
        {
            Container.Bind<Bob>()
                .FromNewScriptableObjectResource(PathPrefix + "Bob").AsCached()
                .WithArguments("test1").NonLazy();

            Initialize();

            Assert.IsEqual(Container.Resolve<Bob>().Arg, "test1");
        }
    }
}
