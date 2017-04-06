using NUnit.Framework;

namespace Zenject.Tests.Other
{
    [TestFixture]
    public class TestTickableManagerParenting
    {
        [SetUp]
        public void ResetCounts()
        {
            RootTickable.TickCount = 0;
            NestedTickableOne.TickCount = 0;
            NestedTickableTwo.TickCount = 0;
            DoublyNestedTickable.TickCount = 0;
        }

        [Test]
        public void TestPausingRoot()
        {
            var container = new DiContainer();

            container.Bind(typeof(TickableManager), typeof(InitializableManager), typeof(DisposableManager))
                .ToSelf().AsSingle().CopyIntoAllSubContainers();

            container.Bind<ITickable>().To<RootTickable>().AsSingle();

            // This is how you add ITickables / etc. within sub containers
            container.BindInterfacesAndSelfTo<NestedKernel>()
                .FromSubContainerResolve().ByMethod(InstallNestedOne).AsSingle();
            container.BindInterfacesAndSelfTo<NestedKernel>()
                .FromSubContainerResolve().ByMethod(InstallNestedTwo).AsSingle();

            var tickManager = container.Resolve<TickableManager>();
            var initManager = container.Resolve<InitializableManager>();
            var disposeManager = container.Resolve<DisposableManager>();

            Assert.AreEqual(0, RootTickable.TickCount);
            Assert.AreEqual(0, NestedTickableOne.TickCount);
            Assert.AreEqual(0, NestedTickableTwo.TickCount);
            Assert.AreEqual(0, DoublyNestedTickable.TickCount);

            initManager.Initialize();
            tickManager.Update();
            disposeManager.Dispose();

            Assert.AreEqual(1, RootTickable.TickCount);
            Assert.AreEqual(1, NestedTickableOne.TickCount);
            Assert.AreEqual(1, NestedTickableTwo.TickCount);
            Assert.AreEqual(1, DoublyNestedTickable.TickCount);

            tickManager.Pause();
            tickManager.Update();

            Assert.AreEqual(1, RootTickable.TickCount);
            Assert.AreEqual(1, NestedTickableOne.TickCount);
            Assert.AreEqual(1, NestedTickableTwo.TickCount);
            Assert.AreEqual(1, DoublyNestedTickable.TickCount);

            tickManager.Resume();
            tickManager.Update();

            Assert.AreEqual(2, RootTickable.TickCount);
            Assert.AreEqual(2, NestedTickableOne.TickCount);
            Assert.AreEqual(2, NestedTickableTwo.TickCount);
            Assert.AreEqual(2, DoublyNestedTickable.TickCount);
        }

        [Test]
        public void TestPausingNested()
        {
            var container = new DiContainer();

            container.Bind(typeof(TickableManager), typeof(InitializableManager), typeof(DisposableManager))
                .ToSelf().AsSingle().CopyIntoAllSubContainers();

            container.Bind<ITickable>().To<RootTickable>().AsSingle();

            // This is how you add ITickables / etc. within sub containers
            container.BindInterfacesAndSelfTo<NestedKernel>()
                .FromSubContainerResolve().ByMethod(InstallNestedOne).AsSingle();
            container.BindInterfacesAndSelfTo<NestedKernel>()
                .FromSubContainerResolve().ByMethod(InstallNestedTwo).AsSingle();
            container.Bind<NestedTickableManagerHolder>()
                .FromSubContainerResolve().ByMethod(InstallNestedTwo).AsSingle();

            var tickManager = container.Resolve<TickableManager>();
            var initManager = container.Resolve<InitializableManager>();
            var disposeManager = container.Resolve<DisposableManager>();

            var nestedTickManager = container.Resolve<NestedTickableManagerHolder>();

            Assert.AreEqual(0, RootTickable.TickCount);
            Assert.AreEqual(0, NestedTickableOne.TickCount);
            Assert.AreEqual(0, NestedTickableTwo.TickCount);
            Assert.AreEqual(0, DoublyNestedTickable.TickCount);

            initManager.Initialize();
            tickManager.Update();
            disposeManager.Dispose();

            Assert.AreEqual(1, RootTickable.TickCount);
            Assert.AreEqual(1, NestedTickableOne.TickCount);
            Assert.AreEqual(1, NestedTickableTwo.TickCount);
            Assert.AreEqual(1, DoublyNestedTickable.TickCount);

            nestedTickManager.TickManager.Pause();
            tickManager.Update();

            Assert.AreEqual(2, RootTickable.TickCount);
            Assert.AreEqual(2, NestedTickableOne.TickCount);
            Assert.AreEqual(1, NestedTickableTwo.TickCount);
            Assert.AreEqual(1, DoublyNestedTickable.TickCount);

            nestedTickManager.TickManager.Resume();
            tickManager.Update();

            Assert.AreEqual(3, RootTickable.TickCount);
            Assert.AreEqual(3, NestedTickableOne.TickCount);
            Assert.AreEqual(2, NestedTickableTwo.TickCount);
            Assert.AreEqual(2, DoublyNestedTickable.TickCount);
        }

        public void InstallNestedOne(DiContainer subContainer)
        {
            subContainer.Bind<NestedKernel>().AsSingle();

            subContainer.Bind<NestedTickableManagerHolder>().AsSingle();

            subContainer.Bind<ITickable>().To<NestedTickableOne>().AsSingle();
        }

        public void InstallNestedTwo(DiContainer subContainer)
        {
            subContainer.Bind<NestedKernel>().AsSingle();

            subContainer.Bind<NestedTickableManagerHolder>().AsSingle();

            subContainer.Bind<ITickable>().To<NestedTickableTwo>().AsSingle();

            subContainer.BindInterfacesAndSelfTo<DoublyNestedKernel>()
                .FromSubContainerResolve().ByMethod(InstallDoublyNested).AsSingle();
        }

        public void InstallDoublyNested(DiContainer subContainer)
        {
            subContainer.Bind<DoublyNestedKernel>().AsSingle();

            subContainer.Bind<NestedTickableManagerHolder>().AsSingle();

            subContainer.Bind<ITickable>().To<DoublyNestedTickable>().AsSingle();
        }

        public class NestedKernel : Kernel
        {
        }

        public class DoublyNestedKernel : Kernel
        {
        }

        public class NestedTickableManagerHolder
        {
            [Inject] public TickableManager TickManager;
        }

        public class RootTickable : ITickable
        {
            public static int TickCount = 0;

            public void Tick()
            {
                TickCount++;
            }
        }

        public class NestedTickableOne : ITickable
        {
            public static int TickCount = 0;

            public void Tick()
            {
                TickCount++;
            }
        }

        public class NestedTickableTwo : ITickable
        {
            public static int TickCount = 0;

            public void Tick()
            {
                TickCount++;
            }
        }

        public class DoublyNestedTickable : ITickable
        {
            public static int TickCount = 0;

            public void Tick()
            {
                TickCount++;
            }
        }

    }
}