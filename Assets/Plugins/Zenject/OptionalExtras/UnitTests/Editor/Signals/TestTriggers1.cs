using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ModestTree;
using Assert=ModestTree.Assert;
using Zenject;

namespace ZenjectSignalsAndSignals.Tests
{
    [TestFixture]
    public class TestTriggers1 : ZenjectIntegrationTestFixture
    {
        [SetUp]
        public void CommonInstall()
        {
            Container.BindInterfacesAndSelfTo<SignalManager>().AsSingle();
        }

        [Test]
        public void Test1()
        {
            Container.DeclareSignal<FooSignal>();

            Initialize();

            var signal = Container.Resolve<FooSignal>();

            bool received = false;
            signal += delegate { received = true; };

            // This is a compiler error
            //signal.Event();

            Assert.That(!received);
            signal.Fire();
            Assert.That(received);
        }

        public class FooSignal : Signal<FooSignal>
        {
        }
    }
}

