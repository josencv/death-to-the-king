using System.Collections.Generic;
using Zenject;
using NUnit.Framework;
using System.Linq;
using ModestTree;
using Assert=ModestTree.Assert;

namespace Zenject.Tests.Other
{
    [TestFixture]
    public class TestGenericContract : ZenjectUnitTestFixture
    {
        class Test1<T>
        {
            public T Data;
        }

        class Test2
        {
        }

        [Test]
        public void TestToSingle()
        {
            Container.Bind(typeof(Test1<>)).AsSingle();

            Container.BindRootResolve<Test1<int>>();
            Container.BindRootResolve<Test1<bool>>();
            Container.BindRootResolve<Test1<string>>();

            var test1 = Container.Resolve<Test1<int>>();
            Assert.That(test1.Data == 0);
            test1.Data = 5;

            var test2 = Container.Resolve<Test1<int>>();

            Assert.That(test2 == test1);
            Assert.That(test1.Data == 5);
        }

        [Test]
        public void TestToTransient()
        {
            Container.Bind(typeof(Test1<>)).AsTransient();

            Container.BindRootResolve<Test1<int>>();
            Container.BindRootResolve<Test1<bool>>();
            Container.BindRootResolve<Test1<string>>();

            var test1 = Container.Resolve<Test1<int>>();
            Assert.That(test1.Data == 0);

            var test2 = Container.Resolve<Test1<int>>();
            Assert.That(test2.Data == 0);
            Assert.That(test2 != test1);

            Container.Resolve<Test1<string>>();
            Container.Resolve<Test1<List<int>>>();
            Container.Resolve<Test1<Test2>>();
        }
    }
}
