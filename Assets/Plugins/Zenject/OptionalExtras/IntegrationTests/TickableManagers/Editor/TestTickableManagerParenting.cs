using ModestTree;
using NUnit.Framework;
using UnityEngine;
using Zenject.Tests.TickableManagers.Parenting;
using Assert = NUnit.Framework.Assert;

namespace Zenject.Tests.TickableManagers
{
    [TestFixture]
    public partial class TestTickableManagerParenting : ZenjectIntegrationTestFixture
    {
        GameObject NestedOnePrefab
        {
            get { return FixtureUtil.GetPrefab("TestParenting/NestedGameObjectContextOne"); }
        }

        GameObject NestedTwoPrefab
        {
            get { return FixtureUtil.GetPrefab("TestParenting/NestedGameObjectContextTwo"); }
        }

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            RootTickable.TickCount = 0;
            NestedTickableOne.TickCount = 0;
            NestedTickableTwo.TickCount = 0;
            DoublyNestedTickable.TickCount = 0;
        }

        [Test]
        public void TestPausingRoot()
        {
            Container.BindInterfacesAndSelfTo<RootTickable>().AsSingle().NonLazy();

            Container.Bind<NestedTickableOne>()
                .FromSubContainerResolve()
                .ByNewPrefab(NestedOnePrefab)
                .AsSingle();

            Container.Bind<NestedTickableTwo>()
                .FromSubContainerResolve()
                .ByNewPrefab(NestedTwoPrefab)
                .AsSingle();

            Initialize();

            Container.Resolve<NestedTickableOne>();
            Container.Resolve<NestedTickableTwo>();

            var tickManager = Container.Resolve<TickableManager>();

            // Contexts (GameObjectContext/SceneContext) use a MonoKernel to
            // hook into the Unity Update. We need to simulate the Update loop.
            var monoKernels = SceneContext.GetComponentsInChildren<MonoKernel>();

            Assert.AreEqual(0, RootTickable.TickCount);
            Assert.AreEqual(0, NestedTickableOne.TickCount);
            Assert.AreEqual(0, NestedTickableTwo.TickCount);
            Assert.AreEqual(0, DoublyNestedTickable.TickCount);

            foreach (var kernel in monoKernels)
            {
                kernel.Update();
            }

            Assert.AreEqual(1, RootTickable.TickCount);
            Assert.AreEqual(1, NestedTickableOne.TickCount);
            Assert.AreEqual(1, NestedTickableTwo.TickCount);
            Assert.AreEqual(1, DoublyNestedTickable.TickCount);

            tickManager.Pause();
            foreach (var kernel in monoKernels)
            {
                kernel.Update();
            }

            Assert.AreEqual(1, RootTickable.TickCount);
            Assert.AreEqual(1, NestedTickableOne.TickCount);
            Assert.AreEqual(1, NestedTickableTwo.TickCount);
            Assert.AreEqual(1, DoublyNestedTickable.TickCount);

            tickManager.Resume();
            foreach (var kernel in monoKernels)
            {
                kernel.Update();
            }

            Assert.AreEqual(2, RootTickable.TickCount);
            Assert.AreEqual(2, NestedTickableOne.TickCount);
            Assert.AreEqual(2, NestedTickableTwo.TickCount);
            Assert.AreEqual(2, DoublyNestedTickable.TickCount);
        }

        [Test]
        public void TestPausingNested()
        {
            Container.BindInterfacesAndSelfTo<RootTickable>().AsSingle().NonLazy();

            Container.Bind<NestedTickableOne>()
                .FromSubContainerResolve()
                .ByNewPrefab(NestedOnePrefab)
                .AsSingle();

            Container.Bind<NestedTickableTwo>()
                .FromSubContainerResolve()
                .ByNewPrefab(NestedTwoPrefab)
                .AsSingle();

            Container.Bind<NestedTickableManagerHolder>()
                .FromSubContainerResolve()
                .ByNewPrefab(NestedTwoPrefab)
                .AsSingle();

            Initialize();

            Container.Resolve<NestedTickableOne>();
            Container.Resolve<NestedTickableTwo>();

            var nestedTickManager = Container.Resolve<NestedTickableManagerHolder>().TickManager;

            // Contexts (GameObjectContext/SceneContext) use a MonoKernel to
            // hook into the Unity Update. We need to simulate the Update loop.
            var monoKernels = SceneContext.GetComponentsInChildren<MonoKernel>();

            Assert.AreEqual(0, RootTickable.TickCount);
            Assert.AreEqual(0, NestedTickableOne.TickCount);
            Assert.AreEqual(0, NestedTickableTwo.TickCount);
            Assert.AreEqual(0, DoublyNestedTickable.TickCount);

            foreach (var kernel in monoKernels)
            {
                kernel.Update();
            }

            Assert.AreEqual(1, RootTickable.TickCount);
            Assert.AreEqual(1, NestedTickableOne.TickCount);
            Assert.AreEqual(1, NestedTickableTwo.TickCount);
            Assert.AreEqual(1, DoublyNestedTickable.TickCount);

            nestedTickManager.Pause();
            foreach (var kernel in monoKernels)
            {
                kernel.Update();
            }

            Assert.AreEqual(2, RootTickable.TickCount);
            Assert.AreEqual(2, NestedTickableOne.TickCount);
            Assert.AreEqual(1, NestedTickableTwo.TickCount);
            Assert.AreEqual(1, DoublyNestedTickable.TickCount);

            nestedTickManager.Resume();
            foreach (var kernel in monoKernels)
            {
                kernel.Update();
            }

            Assert.AreEqual(3, RootTickable.TickCount);
            Assert.AreEqual(3, NestedTickableOne.TickCount);
            Assert.AreEqual(2, NestedTickableTwo.TickCount);
            Assert.AreEqual(2, DoublyNestedTickable.TickCount);
        }
    }
}
