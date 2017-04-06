using System;
using NUnit.Framework;
using Assert=ModestTree.Assert;

namespace Zenject.Tests.BindFeatures
{
    [TestFixture]
    public class TestUnbind : ZenjectUnitTestFixture
    {
        interface ITest
        {
        }

        class Test2 : ITest
        {
        }

        [Test]
        public void Run()
        {
            Container.Bind<ITest>().To<Test2>().AsSingle();

            Assert.IsNotNull(Container.Resolve<ITest>());

            Container.Unbind<ITest>();

            Assert.IsNull(Container.TryResolve<ITest>());
        }
    }
}
