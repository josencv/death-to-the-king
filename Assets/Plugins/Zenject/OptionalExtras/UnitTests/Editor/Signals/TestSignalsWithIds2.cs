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
    public class TestSignalsWithIds2 : ZenjectIntegrationTestFixture
    {
        [SetUp]
        public void CommonInstall()
        {
            Container.BindInterfacesAndSelfTo<SignalManager>().AsSingle();
        }

        [Test]
        public void TestNormal()
        {
            Bar.WasTriggered = false;

            Container.DeclareSignal<DoSomethingSignal>();
            Container.BindSignal<DoSomethingSignal>().To<Bar>(x => x.Execute).AsSingle();

            Initialize();

            Assert.That(!Bar.WasTriggered);

            Container.Resolve<DoSomethingSignal>().Fire();

            Assert.That(Bar.WasTriggered);
        }

        [Test]
        public void TestIdsError()
        {
            Bar.WasTriggered = false;

            Container.DeclareSignal<DoSomethingSignal>().WithId("asdf");
            Container.BindSignal<DoSomethingSignal>().To<Bar>(x => x.Execute).AsSingle();

            Initialize();

            Assert.Throws(() => Container.Resolve<DoSomethingSignal>());

            Assert.That(!Bar.WasTriggered);
            Container.ResolveId<DoSomethingSignal>("asdf").Fire();
            Assert.That(!Bar.WasTriggered);
        }

        [Test]
        public void TestIdsSuccess()
        {
            Bar.WasTriggered = false;
            Bar2.WasTriggered = false;

            Container.DeclareSignal<DoSomethingSignal>().WithId("asdf");
            Container.BindSignal<DoSomethingSignal>().WithId("asdf").To<Bar>(x => x.Execute).AsSingle();

            Container.DeclareSignal<DoSomethingSignal>().WithId("qwer");
            Container.BindSignal<DoSomethingSignal>().WithId("qwer").To<Bar2>(x => x.Execute).AsSingle();

            Initialize();

            var cmd1 = Container.ResolveId<DoSomethingSignal>("asdf");
            var cmd2 = Container.ResolveId<DoSomethingSignal>("qwer");

            Assert.That(!Bar.WasTriggered);
            Assert.That(!Bar2.WasTriggered);

            cmd1.Fire();

            Assert.That(Bar.WasTriggered);
            Assert.That(!Bar2.WasTriggered);

            Bar.WasTriggered = false;

            cmd2.Fire();

            Assert.That(!Bar.WasTriggered);
            Assert.That(Bar2.WasTriggered);
        }

        public class DoSomethingSignal : Signal<DoSomethingSignal>
        {
        }

        public class Bar
        {
            public static bool WasTriggered
            {
                get;
                set;
            }

            public void Execute()
            {
                WasTriggered = true;
            }
        }

        public class Bar2
        {
            public static bool WasTriggered
            {
                get;
                set;
            }

            public void Execute()
            {
                WasTriggered = true;
            }
        }
    }
}

