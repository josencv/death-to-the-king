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
    public class TestSignalsSubcontainers : ZenjectIntegrationTestFixture
    {
        [SetUp]
        public void CommonInstall()
        {
            Container.BindInterfacesAndSelfTo<SignalManager>().AsSingle();
        }

        [Test]
        public void TestMissingDeclaration()
        {
            Container.BindSignal<DoSomethingSignal>().To<Bar>(x => x.Execute).AsSingle();
            Initialize();
        }

        [Test]
        public void TestDeclarationBelowHandler()
        {
            Bar.WasTriggered = false;

            Container.BindSignal<DoSomethingSignal>().To<Bar>(x => x.Execute).AsSingle();

            Container.BindInterfacesAndSelfTo<Foo>().FromSubContainerResolve().ByMethod(InstallFoo).AsSingle().NonLazy();

            Initialize();

            Assert.Throws(() => Container.Resolve<DoSomethingSignal>());

            var foo = Container.Resolve<Foo>();

            Assert.That(!Bar.WasTriggered);
            foo.Trigger();
            Assert.That(Bar.WasTriggered);
        }

        static void InstallFoo(DiContainer container)
        {
            container.Bind<Foo>().AsSingle();
            container.DeclareSignal<DoSomethingSignal>();
        }

        [Test]
        public void TestDeclarationAboveHandler()
        {
            Bar.WasTriggered = false;

            Container.DeclareSignal<DoSomethingSignal>();

            Container.BindInterfacesAndSelfTo<Foo>().FromSubContainerResolve().ByMethod(InstallFoo2).AsSingle().NonLazy();

            Initialize();

            var cmd = Container.Resolve<DoSomethingSignal>();
            Container.Resolve<Foo>();

            Assert.That(!Bar.WasTriggered);
            cmd.Fire();
            Assert.That(Bar.WasTriggered);
        }

        static void InstallFoo2(DiContainer container)
        {
            container.Bind<Foo>().AsSingle();
            container.BindSignal<DoSomethingSignal>().To<Bar>(x => x.Execute).AsSingle();
        }

        public class Foo : Kernel
        {
            readonly DoSomethingSignal _command;

            public Foo(DoSomethingSignal command)
            {
                _command = command;
            }

            public void Trigger()
            {
                _command.Fire();
            }
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
    }
}


