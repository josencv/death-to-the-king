using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using ModestTree;
using Assert=ModestTree.Assert;
using Zenject.Tests.Installers.Installers;

namespace Zenject.Tests.Installers
{
    [TestFixture]
    public class TestInstallers : ZenjectIntegrationTestFixture
    {
        [Test]
        public void TestZeroArgs()
        {
            FooInstaller.Install(Container);

            Initialize();

            FixtureUtil.AssertResolveCount<Foo>(Container, 1);
        }

        [Test]
        public void TestOneArg()
        {
            BarInstaller.Install(Container, "blurg");

            Initialize();

            Assert.IsEqual(Container.Resolve<string>(), "blurg");
        }

        [Test]
        public void TestThreeArgs()
        {
            QuxInstaller.Install(Container, "blurg", 2.0f, 1);

            Initialize();

            Assert.IsEqual(Container.Resolve<string>(), "blurg");
        }
    }
}

