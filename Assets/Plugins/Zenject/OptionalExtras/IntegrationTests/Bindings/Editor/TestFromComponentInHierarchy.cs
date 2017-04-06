using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using ModestTree;
using Assert=ModestTree.Assert;

namespace Zenject.Tests.Bindings
{
    [TestFixture]
    public class TestFromComponentInHierarchy : ZenjectIntegrationTestFixture
    {
        Foo _foo;
        Bar _bar;

        void InitScene()
        {
            var root = new GameObject();

            var child1 = new GameObject();
            child1.transform.SetParent(root.transform);

            var child2 = new GameObject();
            child2.transform.SetParent(root.transform);

            _foo = child2.AddComponent<Foo>();
            _bar = child2.AddComponent<Bar>();
        }

        public override void SetUp()
        {
            InitScene();

            base.SetUp();
        }

        [Test]
        public void RunTest()
        {
            Container.Bind<Foo>().FromComponentInHierarchy();

            Initialize();

            Assert.IsEqual(_bar.Foo, _foo);
        }

        public class Foo : MonoBehaviour
        {
        }

        public class Bar : MonoBehaviour
        {
            [Inject]
            public Foo Foo;
        }
    }
}


