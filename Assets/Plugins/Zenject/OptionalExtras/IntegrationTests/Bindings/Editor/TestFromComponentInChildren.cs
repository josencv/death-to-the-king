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
    public class TestFromComponentInChildren : ZenjectIntegrationTestFixture
    {
        Root _root;
        Child _child1;
        Child _child2;
        Grandchild _grandchild;

        void InitScene()
        {
            _root = new GameObject().AddComponent<Root>();

            _child1 = new GameObject().AddComponent<Child>();
            _child1.transform.SetParent(_root.transform);

            _child2 = new GameObject().AddComponent<Child>();
            _child2.transform.SetParent(_root.transform);

            _grandchild = new GameObject().AddComponent<Grandchild>();
            _grandchild.transform.SetParent(_child1.transform);
        }

        public override void SetUp()
        {
            InitScene();

            base.SetUp();
        }

        [Test]
        public void RunTest()
        {
            Container.Bind<Grandchild>().FromComponentInChildren();
            Container.Bind<Child>().FromComponentInChildren();

            Initialize();

            Assert.IsEqual(_root.Grandchild, _grandchild);
            Assert.IsEqual(_root.Childs[0], _child1);
            Assert.IsEqual(_root.Childs[1], _child2);
        }

        public class Root : MonoBehaviour
        {
            [Inject]
            public Grandchild Grandchild;

            [Inject]
            public List<Child> Childs;
        }

        public class Child : MonoBehaviour
        {
        }

        public class Grandchild : MonoBehaviour
        {
        }
    }
}


