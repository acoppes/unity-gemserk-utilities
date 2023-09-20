using Game.Components;
using Game.Systems;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using NUnit.Framework;
using UnityEngine;

namespace Game.Editor.Tests
{
    public class ControllableInputCopySystemTests
    {
        private ControllableInputCopySystem controllableInputCopySystem;
        private EcsWorld ecsWorld;
        private EcsSystems ecsSystems;
        
        [SetUp]
        public void BeforeEach()
        {
            var gameObject = new GameObject();
            controllableInputCopySystem = gameObject.AddComponent<ControllableInputCopySystem>();

            ecsWorld = new EcsWorld();
            ecsSystems = new EcsSystems(ecsWorld);

            ecsSystems.Add(controllableInputCopySystem);
            
            ecsSystems.Init();
            ecsSystems.Inject();
        }

        private int CreatePlayerControlEntity()
        {
            var e = ecsWorld.NewEntity();
            
            ref var inputComponent = ref ecsWorld.GetPool<InputComponent>().Add(e);
            inputComponent = InputComponent.Default();
            
            ref var playerControl = ref ecsWorld.GetPool<PlayerControlComponent>().Add(e);
            playerControl.controlId = 1;
            
            return e;
        }

        private int CreateTestEntity()
        {
            var e = ecsWorld.NewEntity();
            ref var inputComponent = ref ecsWorld.GetPool<InputComponent>().Add(e);
            inputComponent = InputComponent.Default();

            ref var controllable = ref ecsWorld.GetPool<ControllableByComponent>().Add(e);
            controllable.controllableType = 
                ControllableByComponent.ControllableType.Player;
            controllable.playerControlId = 1;
            
            return e;
        }
        
        [Test]
        public void ShouldCopy_FromInput_ToEntityWithInput()
        {
            var e0 = CreatePlayerControlEntity(); 
            var e1 = CreateTestEntity();

            ecsWorld.GetPool<InputComponent>().Get(e0).button1().isPressed = true;
            
            ecsSystems.Run();
            
            Assert.IsTrue(ecsWorld.GetPool<InputComponent>().Get(e1).button1().isPressed);
        }
        
        [Test]
        public void ShouldCopy_FromInput_DontShareInstance()
        {
            var e0 = CreatePlayerControlEntity(); 
            var e1 = CreateTestEntity();

            ecsWorld.GetPool<InputComponent>().Get(e0).actions = null;
            
            ecsSystems.Run();
            
            Assert.IsNotNull(ecsWorld.GetPool<InputComponent>().Get(e1).actions);
        }
        
        [Test]
        public void ShouldIgnore_IfNotControllable()
        {
            var e0 = CreatePlayerControlEntity(); 
            var e1 = CreateTestEntity();

            ecsWorld.GetPool<InputComponent>().Get(e0).button1().isPressed = true;
            ecsWorld.GetPool<ControllableByComponent>().Get(e1).controllableType =
                ControllableByComponent.ControllableType.Nothing;
            
            ecsSystems.Run();
            
            Assert.IsFalse(ecsWorld.GetPool<InputComponent>().Get(e1).button1().isPressed);
        }
        
        [Test]
        public void ShouldIgnore_IfControllable_ButByAnotherPlayer()
        {
            var e0 = CreatePlayerControlEntity(); 
            var e1 = CreateTestEntity();

            ecsWorld.GetPool<InputComponent>().Get(e0).button1().isPressed = true;
            ecsWorld.GetPool<ControllableByComponent>().Get(e1).playerControlId = 2;
            
            ecsSystems.Run();
            
            Assert.IsFalse(ecsWorld.GetPool<InputComponent>().Get(e1).button1().isPressed);
        }
    }
}