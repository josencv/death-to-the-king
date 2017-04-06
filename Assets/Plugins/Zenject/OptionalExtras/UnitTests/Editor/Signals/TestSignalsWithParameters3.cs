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
    public class TestSignalsWithParameters3 : ZenjectIntegrationTestFixture
    {
        [SetUp]
        public void CommonInstall()
        {
            Container.BindInterfacesAndSelfTo<SignalManager>().AsSingle();
        }

        [Test]
        public void TestParameters0()
        {
            Bar0.HasFired = false;

            Container.DeclareSignal<DoSomethingSignal0>();
            Container.BindSignal<DoSomethingSignal0>()
                .To<Bar0>((h) => h.Execute()).AsSingle();

            Initialize();

            var cmd = Container.Resolve<DoSomethingSignal0>();

            cmd.Fire();

            Assert.That(Bar0.HasFired);
        }

        [Test]
        public void TestParameters1()
        {
            Bar1.Value = null;

            Container.DeclareSignal<DoSomethingSignal1>();
            Container.BindSignal<string, DoSomethingSignal1>()
                .To<Bar1>((h, p1) => h.Execute(p1)).AsSingle();

            Initialize();

            var cmd = Container.Resolve<DoSomethingSignal1>();

            Assert.IsNull(Bar1.Value);

            cmd.Fire("asdf");

            Assert.IsEqual(Bar1.Value, "asdf");
        }

        [Test]
        public void TestParameters2()
        {
            Bar2.Value1 = null;
            Bar2.Value2 = 0;

            Container.DeclareSignal<DoSomethingSignal2>();
            Container.BindSignal<string, object, DoSomethingSignal2>()
                .To<Bar2>((h, p1, p2) => h.Execute(p1, p2)).AsSingle();

            Initialize();

            var cmd = Container.Resolve<DoSomethingSignal2>();

            Assert.IsNull(Bar2.Value1);
            Assert.IsEqual(Bar2.Value2, 0);

            cmd.Fire("asdf", 4);

            Assert.IsEqual(Bar2.Value1, "asdf");
            Assert.IsEqual(Bar2.Value2, 4);
        }

        [Test]
        public void TestParameters3()
        {
            Bar3.Value1 = null;
            Bar3.Value2 = 0;
            Bar3.Value3 = 0.0f;

            Container.DeclareSignal<DoSomethingSignal3>();
            Container.BindSignal<string, object, object, DoSomethingSignal3>()
                .To<Bar3>((h, p1, p2, p3) => h.Execute(p1, p2, p3)).AsSingle();

            Initialize();

            var cmd = Container.Resolve<DoSomethingSignal3>();

            Assert.IsNull(Bar3.Value1);
            Assert.IsEqual(Bar3.Value2, 0);
            Assert.IsEqual(Bar3.Value3, 0.0f);

            cmd.Fire("asdf", 4, 7.2f);

            Assert.IsEqual(Bar3.Value1, "asdf");
            Assert.IsEqual(Bar3.Value2, 4);
            Assert.IsEqual(Bar3.Value3, 7.2f);
        }

        [Test]
        public void TestParameters4()
        {
            Bar4.Value1 = null;
            Bar4.Value2 = 0;
            Bar4.Value3 = 0.0f;
            Bar4.Value4 = "0";

            Container.DeclareSignal<DoSomethingSignal4>();
            Container.BindSignal<string, object, object, string, DoSomethingSignal4>()
                .To<Bar4>((h, p1, p2, p3, p4) => h.Execute(p1, p2, p3, p4)).AsSingle();

            Initialize();

            var cmd = Container.Resolve<DoSomethingSignal4>();

            Assert.IsNull(Bar4.Value1);
            Assert.IsEqual(Bar4.Value2, 0);
            Assert.IsEqual(Bar4.Value3, 0.0f);
            Assert.IsEqual(Bar4.Value4, "0");

            cmd.Fire("asdf", 4, 7.2f, "z");

            Assert.IsEqual(Bar4.Value1, "asdf");
            Assert.IsEqual(Bar4.Value2, 4);
            Assert.IsEqual(Bar4.Value3, 7.2f);
            Assert.IsEqual(Bar4.Value4, "z");
        }

        public class DoSomethingSignal0 : Signal<DoSomethingSignal0> { }
        public class DoSomethingSignal1 : Signal<string, DoSomethingSignal1> { }
        public class DoSomethingSignal2 : Signal<string, object, DoSomethingSignal2> { }
        public class DoSomethingSignal3 : Signal<string, object, object, DoSomethingSignal3> { }
        public class DoSomethingSignal4 : Signal<string, object, object, string, DoSomethingSignal4> { }

        public class Bar0
        {
            public static bool HasFired;

            public void Execute()
            {
                HasFired = true;
            }
        }

        public class Bar1
        {
            public static string Value;

            public void Execute(string value)
            {
                Value = value;
            }
        }

        public class Bar2
        {
            public static string Value1;
            public static object Value2;

            public void Execute(string value1, object value2)
            {
                Value1 = value1;
                Value2 = value2;
            }
        }

        public class Bar3
        {
            public static string Value1;
            public static object Value2;
            public static object Value3;

            public void Execute(string value1, object value2, object value3)
            {
                Value1 = value1;
                Value2 = value2;
                Value3 = value3;
            }
        }

        public class Bar4
        {
            public static string Value1;
            public static object Value2;
            public static object Value3;
            public static string Value4;

            public void Execute(string value1, object value2, object value3, string value4)
            {
                Value1 = value1;
                Value2 = value2;
                Value3 = value3;
                Value4 = value4;
            }
        }
    }
}


