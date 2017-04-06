using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using ModestTree;
using Assert=ModestTree.Assert;
using Zenject.Tests.Installers.ScriptableObjectInstallers;

namespace Zenject.Tests.Installers
{
    [TestFixture]
    public class TestScriptableObjectInstallers : ZenjectIntegrationTestFixture
    {
        [Test]
        public void TestBadResourcePath()
        {
            Assert.Throws(() => FooInstaller.InstallFromResource("TestScriptableObjectInstallers/SDFSDFSDF", Container));
            Initialize();
        }

        [Test]
        public void TestZeroArgs()
        {
            FooInstaller.InstallFromResource("TestScriptableObjectInstallers/FooInstaller", Container);

            Initialize();

            FixtureUtil.AssertResolveCount<Foo>(Container, 1);
        }

        [Test]
        public void TestOneArg()
        {
            BarInstaller.InstallFromResource("TestScriptableObjectInstallers/BarInstaller", Container, "blurg");

            Initialize();

            Assert.IsEqual(Container.Resolve<string>(), "blurg");
        }

        [Test]
        public void TestThreeArgs()
        {
            QuxInstaller.InstallFromResource("TestScriptableObjectInstallers/QuxInstaller", Container, "blurg", 2.0f, 1);

            Initialize();

            Assert.IsEqual(Container.Resolve<string>(), "blurg");
        }
    }
}

