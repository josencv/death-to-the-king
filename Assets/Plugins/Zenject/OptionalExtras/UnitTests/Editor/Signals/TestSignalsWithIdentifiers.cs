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
    public class TestSignalsWithIdentifiers : ZenjectIntegrationTestFixture
    {
        [SetUp]
        public void CommonInstall()
        {
            Container.BindInterfacesAndSelfTo<SignalManager>().AsSingle();
        }

        [Test]
        public void RunTest()
        {
            Container.DeclareSignal<SomethingHappenedSignal>();

            Container.Bind<Foo>().AsSingle();
            Container.Bind<Bar>().AsSingle();

            Container.DeclareSignal<SomethingHappenedSignal>().WithId("special");

            Container.Bind<FooSpecial>().AsSingle();
            Container.Bind<BarSpecial>().AsSingle();

            Initialize();

            var foo = Container.Resolve<Foo>();
            var bar = Container.Resolve<Bar>();

            var fooSpecial = Container.Resolve<FooSpecial>();
            var barSpecial = Container.Resolve<BarSpecial>();

            bar.Initialize();
            barSpecial.Initialize();

            Assert.IsNull(bar.ReceivedValue);
            Assert.IsNull(barSpecial.ReceivedValue);

            foo.DoSomething("asdf");

            Assert.IsEqual(bar.ReceivedValue, "asdf");
            Assert.IsNull(barSpecial.ReceivedValue);

            bar.ReceivedValue = null;

            fooSpecial.DoSomething("zxcv");

            Assert.IsEqual(barSpecial.ReceivedValue, "zxcv");
            Assert.IsNull(bar.ReceivedValue);

            bar.Dispose();
            barSpecial.Dispose();
        }

        public class SomethingHappenedSignal : Signal<string, SomethingHappenedSignal>
        {
        }

        public class Foo
        {
            SomethingHappenedSignal _signal;

            public Foo(
                SomethingHappenedSignal signal)
            {
                _signal = signal;
            }

            public void DoSomething(string value)
            {
                _signal.Fire(value);
            }
        }

        public class Bar
        {
            SomethingHappenedSignal _signal;

            public Bar(SomethingHappenedSignal signal)
            {
                _signal = signal;
            }

            public string ReceivedValue
            {
                get;
                set;
            }

            public void Initialize()
            {
                _signal += OnStarted;
            }

            public void Dispose()
            {
                _signal -= OnStarted;
            }

            void OnStarted(string value)
            {
                ReceivedValue = value;
            }
        }

        public class FooSpecial
        {
            SomethingHappenedSignal _signal;

            public FooSpecial(
                [Inject(Id = "special")]
                SomethingHappenedSignal signal)
            {
                _signal = signal;
            }

            public void DoSomething(string value)
            {
                _signal.Fire(value);
            }
        }

        public class BarSpecial
        {
            SomethingHappenedSignal _signal;

            public BarSpecial(
                [Inject(Id = "special")]
                SomethingHappenedSignal signal)
            {
                _signal = signal;
            }

            public string ReceivedValue
            {
                get;
                set;
            }

            public void Initialize()
            {
                _signal += OnStarted;
            }

            public void Dispose()
            {
                _signal -= OnStarted;
            }

            void OnStarted(string value)
            {
                ReceivedValue = value;
            }
        }
    }
}

