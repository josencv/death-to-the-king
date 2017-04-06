using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using ModestTree;
using Assert=ModestTree.Assert;


namespace Zenject.Tests
{
    public static class FixtureUtil
    {
        public static GameObject GetPrefab(string resourcePath)
        {
            var prefab = (GameObject)Resources.Load(resourcePath);
            Assert.IsNotNull(prefab, "Expected to find prefab at '{0}'", resourcePath);
            return prefab;
        }

        public static void AssertNumGameObjectsWithName(
            DiContainer container, string name, int expectedNumGameObjects)
        {
            var context = container.Resolve<Context>();
            Assert.IsEqual(
                context.transform.Cast<Transform>()
                .Where(x => x.name == name).Count(),
                expectedNumGameObjects);
        }

        public static void AssertNumGameObjects(
            DiContainer container, int expectedNumGameObjects)
        {
            var context = container.Resolve<Context>();
            Assert.IsEqual(context.transform.childCount, expectedNumGameObjects);
        }

        public static void AssertComponentCount<TComponent>(
            DiContainer container, int expectedNumComponents)
        {
            Assert.That(typeof(TComponent).DerivesFromOrEqual<Component>()
                || typeof(TComponent).IsAbstract());

            var actualCount = GetDescendants(container.Resolve<Context>().transform)
                .SelectMany(x => x.GetComponents<TComponent>()).Count();

            Assert.IsEqual(actualCount, expectedNumComponents,
                "Expected to find '{0}' components of type '{1}' but instead found '{2}'"
                .Fmt(expectedNumComponents, typeof(TComponent).Name(), actualCount));
        }

        // Normally we would just use GetComponentsInChildren instead of this
        // but for some reason this doesn't work when run in the tests
        // However, it does work with Unity 5.5, just not Unity 5.4.x, so I'm thinking
        // it's a Unity bug
        static IEnumerable<Transform> GetDescendants(Transform parent)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                var child = parent.GetChild(i);
                yield return child;

                foreach (var grandChild in GetDescendants(child))
                {
                    yield return grandChild;
                }
            }
        }

        public static void AssertResolveCount<TContract>(
            DiContainer container, int expectedNum)
        {
            var actualCount = container.ResolveAll<TContract>().Count;
            Assert.That(actualCount == expectedNum,
                "Expected to find '{0}' instances of type '{1}' but instead found '{2}'",
                expectedNum, typeof(TContract).Name(), actualCount);
        }

        public static void CallFactoryCreateMethod<TValue, TFactory>(DiContainer container)
            where TFactory : Factory<TValue>
        {
            container.Resolve<TFactory>().Create();
        }
    }
}
