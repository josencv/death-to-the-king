using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using ModestTree;
using Assert=ModestTree.Assert;

namespace Zenject.Tests.Bindings.FromComponentInHierarchyGameObjectContext
{
    [TestFixture]
    public class TestFromComponentInHierarchyGameObjectContext : ZenjectIntegrationTestFixture
    {
        GameObject FooPrefab
        {
            get
            {
                return FixtureUtil.GetPrefab("TestFromComponentInHierarchyGameObjectContext/Foo");
            }
        }

        void InitScene()
        {
            new GameObject().AddComponent<Gorp>();
            new GameObject().AddComponent<Gorp>();
        }

        public override void SetUp()
        {
            InitScene();
            base.SetUp();
        }

        [Test]
        public void TestCorrectHierarchy()
        {
            Container.Bind<Foo>().FromSubContainerResolve()
                .ByNewPrefab(FooPrefab).AsSingle().NonLazy();

            Initialize();

            var foo = Container.Resolve<Foo>();

            Assert.IsNotNull(foo.Gorp);
            Assert.IsEqual(foo.gameObject.GetComponentsInChildren<Gorp>().Single(), foo.Gorp);
        }
    }
}
