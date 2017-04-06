using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using ModestTree;
using UnityEngine.SceneManagement;
using Assert=ModestTree.Assert;

namespace Zenject.Tests.SceneParenting
{
    [TestFixture]
    public class TestSceneParentingName
    {
        const string CommonFolderPath = "Assets/Plugins/Zenject/OptionalExtras/IntegrationTests/SceneLoading/Parenting";
        const string ParentScenePath = CommonFolderPath + "/ParentScene.unity";
        const string ChildScenePath = CommonFolderPath + "/ChildScene.unity";

        Scene _childScene;
        Scene _parentScene;

        [Test]
        public void TestValidationPass()
        {
            ZenUnityEditorUtil.ValidateCurrentSceneSetup();
        }

        [Test]
        public void TestValidationFail()
        {
            _parentScene.GetRootGameObjects().Where(x => x.name == "Foo")
                .Single().GetComponent<ZenjectBinding>().enabled = false;

            Assert.Throws(() => ZenUnityEditorUtil.ValidateCurrentSceneSetup());
        }

        [Test]
        public void TestSuccess()
        {
            ZenUnityEditorUtil.RunCurrentSceneSetup();
        }

        [Test]
        public void TestFail()
        {
            _parentScene.GetRootGameObjects().Where(x => x.name == "Foo")
                .Single().GetComponent<ZenjectBinding>().enabled = false;

            Assert.Throws(() => ZenUnityEditorUtil.RunCurrentSceneSetup());
        }

        [SetUp]
        public void SetUp()
        {
            ClearScene();

            _parentScene = EditorSceneManager.OpenScene(ParentScenePath, OpenSceneMode.Additive);
            _childScene = EditorSceneManager.OpenScene(ChildScenePath, OpenSceneMode.Additive);
        }

        [TearDown]
        public void TearDown()
        {
            EditorSceneManager.CloseScene(_childScene, true);
            EditorSceneManager.CloseScene(_parentScene, true);

            ClearScene();
        }

        void ClearScene()
        {
            var scene = EditorSceneManager.GetActiveScene();

            // This is the temp scene that unity creates for EditorTestRunner
            Assert.IsEqual(scene.name, "");

            foreach (var gameObject in scene.GetRootGameObjects())
            {
                GameObject.DestroyImmediate(gameObject);
            }
        }
    }
}
