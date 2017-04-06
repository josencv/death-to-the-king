using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using ModestTree;
using Assert=ModestTree.Assert;
using Zenject.Tests.Installers.MonoInstallers;

namespace Zenject.Tests.Installers
{
    [TestFixture]
    public class TestMonoInstallers : ZenjectIntegrationTestFixture
    {
        [Test]
        public void TestBadResourcePath()
        {
            Assert.Throws(() => FooInstaller.InstallFromResource("TestMonoInstallers/SDFSDFSDF", Container));
            Initialize();
        }

        [Test]
        public void TestZeroArgs()
        {
            FooInstaller.InstallFromResource("TestMonoInstallers/FooInstaller", Container);

            Initialize();

            FixtureUtil.AssertResolveCount<Foo>(Container, 1);
        }

        [Test]
        public void TestOneArg()
        {
            BarInstaller.InstallFromResource("TestMonoInstallers/BarInstaller", Container, "blurg");

            Initialize();

            Assert.IsEqual(Container.Resolve<string>(), "blurg");
        }

        [Test]
        public void TestThreeArgs()
        {
            QuxInstaller.InstallFromResource("TestMonoInstallers/QuxInstaller", Container, "blurg", 2.0f, 1);

            Initialize();

            Assert.IsEqual(Container.Resolve<string>(), "blurg");
        }
    }
}

