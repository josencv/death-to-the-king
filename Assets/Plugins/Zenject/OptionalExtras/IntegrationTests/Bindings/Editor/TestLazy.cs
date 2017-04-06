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
    public class TestLazy : ZenjectIntegrationTestFixture
    {
        [Test]
        public void Test1()
        {
            Bar.InstanceCount = 0;

            Container.Bind<Bar>().AsSingle();
            Container.Bind<Foo>().AsSingle();

            Initialize();

            var foo = Container.Resolve<Foo>();

            Assert.IsEqual(Bar.InstanceCount, 0);

            foo.DoIt();

            Assert.IsEqual(Bar.InstanceCount, 1);
        }

        [Test]
        public void Test2()
        {
            Container.Bind<Foo>().AsSingle().NonLazy();

            Initialize();

            var foo = Container.Resolve<Foo>();
            Assert.Throws(() => foo.DoIt());
        }

        [Test]
        [ValidateOnly]
        public void Test3()
        {
            Container.Bind<Foo>().AsSingle().NonLazy();

            Assert.Throws(() => Initialize());
        }

        [Test]
        [ValidateOnly]
        public void Test4()
        {
            Container.Bind<Foo>().AsSingle().NonLazy();
            Container.Bind<Bar>().AsSingle();
            Initialize();
        }

        public class Bar
        {
            public static int InstanceCount = 0;

            public Bar()
            {
                InstanceCount++;
            }

            public void DoIt()
            {
            }
        }

        public class Foo
        {
            readonly Lazy<Bar> _bar;

            public Foo(Lazy<Bar> bar)
            {
                _bar = bar;
            }

            public void DoIt()
            {
                _bar.Value.DoIt();
            }
        }
    }
}

