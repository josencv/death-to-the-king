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
    public class TestSignalsSixParams : ZenjectIntegrationTestFixture
    {
        [SetUp]
        public void CommonInstall()
        {
            Container.BindInterfacesAndSelfTo<SignalManager>().AsSingle();
        }

        [Test]
        public void RunTest()
        {
            Container.Bind<Foo>().AsSingle();
            Container.Bind<Bar>().AsSingle();

            Container.DeclareSignal<SomethingHappenedSignal>();

            Initialize();

            var foo = Container.Resolve<Foo>();
            var bar = Container.Resolve<Bar>();
            bar.Initialize();

            Assert.IsNull(bar.ReceivedValue);
            foo.DoSomething("asdf", null, null, "zxcv");
            Assert.IsEqual(bar.ReceivedValue, "zxcv");

            bar.Dispose();
        }

        public class SomethingHappenedSignal : Signal<string, object, object, string, SomethingHappenedSignal>
        {
        }

        public class Foo
        {
            SomethingHappenedSignal _signal;

            public Foo(SomethingHappenedSignal signal)
            {
                _signal = signal;
            }

            public void DoSomething(string value1, object value2, object value3, string value4)
            {
                _signal.Fire(value1, value2, value3, value4);
            }
        }

        public class Bar
        {
            SomethingHappenedSignal _signal;
            string _receivedValue;

            public Bar(SomethingHappenedSignal signal)
            {
                _signal = signal;
            }

            public string ReceivedValue
            {
                get
                {
                    return _receivedValue;
                }
            }

            public void Initialize()
            {
                _signal += OnStarted;
            }

            public void Dispose()
            {
                _signal -= OnStarted;
            }

            void OnStarted(string value1, object value2, object value3, string value4)
            {
                _receivedValue = value4;
            }
        }
    }
}

