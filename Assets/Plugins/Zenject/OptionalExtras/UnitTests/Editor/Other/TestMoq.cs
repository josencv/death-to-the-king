using System;
using NUnit.Framework;
//using Moq;
using ModestTree;
using Assert=ModestTree.Assert;

namespace Zenject.Tests.Other
{
    [TestFixture]
    public class TestMoq
    {
        [Test]
        public void TestCase1()
        {
            //var container = new DiContainer();
            //container.Bind<IFoo>().ToMock();

            //Assert.That(container.ValidateResolve<IFoo>().IsEmpty());
            //var foo = container.Resolve<IFoo>();

            //Assert.IsEqual(foo.GetBar(), 0);
        }

        public interface IFoo
        {
            int GetBar();
        }
    }
}
