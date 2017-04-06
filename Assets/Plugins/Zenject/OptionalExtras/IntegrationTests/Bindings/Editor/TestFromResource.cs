﻿using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using ModestTree;
using Assert=ModestTree.Assert;

namespace Zenject.Tests.Bindings
{
    [TestFixture]
    public class TestFromResource : ZenjectIntegrationTestFixture
    {
        const string ResourcePath = "TestFromResource/TestTexture";
        const string ResourcePath2 = "TestFromResource/TestTexture2";

        [Test]
        public void TestBasic()
        {
            Container.Bind<Texture>().FromResource(ResourcePath);

            Container.Bind<Runner>().FromNewComponentOnNewGameObject().AsSingle().WithArguments(1).NonLazy();

            Initialize();
        }

        [Test]
        public void TestTransient()
        {
            Container.Bind<Texture>().FromResource(ResourcePath).AsTransient();
            Container.Bind<Texture>().FromResource(ResourcePath);
            Container.Bind<Texture>().To<Texture>().FromResource(ResourcePath);

            Container.Bind<Runner>().FromNewComponentOnNewGameObject().AsSingle().WithArguments(3).NonLazy();

            Initialize();
        }

        [Test]
        public void TestCached()
        {
            Container.Bind<Texture>().FromResource(ResourcePath).AsCached();

            Container.Bind<Runner>().FromNewComponentOnNewGameObject().AsSingle().WithArguments(1).NonLazy();

            Initialize();
        }

        [Test]
        public void TestSingle()
        {
            Container.Bind<Texture>().FromResource(ResourcePath).AsSingle();
            Container.Bind<Texture>().FromResource(ResourcePath).AsSingle();

            Container.Bind<Runner>().FromNewComponentOnNewGameObject().AsSingle().WithArguments(2).NonLazy();

            Initialize();
        }

        [Test]
        public void TestSingleWithError()
        {
            Container.Bind<Texture>().FromResource(ResourcePath).AsSingle();
            Container.Bind<Texture>().FromResource(ResourcePath2).AsSingle();

            Assert.Throws(() => Container.FlushBindings());

            Initialize();
        }

        public class Runner : MonoBehaviour
        {
            List<Texture> _textures;

            [Inject]
            public void Construct(List<Texture> textures, int expectedAmount)
            {
                _textures = textures;

                Assert.IsEqual(textures.Count, expectedAmount);
            }

            void OnGUI()
            {
                int top = 0;

                foreach (var tex in _textures)
                {
                    var rect = new Rect(0, top, Screen.width * 0.5f, Screen.height * 0.5f);

                    GUI.DrawTexture(rect, tex);

                    top += 200;
                }
            }
        }
    }
}
