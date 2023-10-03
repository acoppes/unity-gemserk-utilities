using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
#if UNITY_EDITOR
using Leopotam.EcsLite.UnityEditor;
#endif
using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{
    public interface IEntityCreatedHandler
    {
        void OnEntityCreated(World world, Entity entity);
    }

    public interface IEntityDestroyedHandler
    {
        void OnEntityDestroyed(World world, Entity entity);
    }

    public class World : CachedObjectBehaviour<World>
    {
        [SerializeField]
        private bool disableLeoEcsDebug;
        
        [SerializeField]
        private Transform fixedUpdateParent, updateParent, lateUpdateParent;
        
        internal EcsWorld world;

        public readonly WorldSharedData sharedData = new WorldSharedData();

        private EcsSystems fixedUpdateSystems, updateSystems, lateUpdateSystems;

        private bool initialized;

        public Action<World, Entity> onEntityCreated, onEntityDestroyed;

        private readonly IList<IEntityCreatedHandler> entityCreatedHandlers = new List<IEntityCreatedHandler>();
        private readonly IList<IEntityDestroyedHandler> entityDestroyedHandlers = new List<IEntityDestroyedHandler>();

        public EcsWorld EcsWorld => world;

        private Entity CreateEmptyEntity()
        {
            var entity = world.NewEntity();
            return GetEntity(entity);
        }

        public Entity CreateEntity(IEntityDefinition definition = null, IEnumerable<IEntityInstanceParameter> parametersList = null)
        {
            var entity = CreateEmptyEntity();

            if (definition != null)
            {
                AddComponent(entity, new EntityDefinitionComponent
                {
                    definition = definition,
                    parameters = parametersList
                });
                
                definition.Apply(this, entity);
            }
            
            if (parametersList != null)
            {
                foreach (var parameters in parametersList)
                {
                    parameters.Apply(this, entity);
                }
            }
            
            OnEntityCreated(entity);
            
            return entity;
        }

        public void DestroyEntity(Entity entity)
        {
            OnEntityDestroyed(entity);
            world.DelEntity(entity);
        }

        public bool Exists(Entity entity)
        {
            if (entity == Entity.NullEntity)
                return false;
            
            return entity.ecsGeneration == world.GetEntityGen(entity.ecsEntity);
        }
        
        public Entity GetEntity(int entity)
        {
            return Entity.Create(this, entity, world.GetEntityGen(entity));
        }

        public ref T AddComponent<T>(Entity entity) where T : struct
        {
            return ref world.GetPool<T>().Add(entity);
        }
        
        public void RemoveComponent<T>(Entity entity) where T : struct
        {
            world.GetPool<T>().Del(entity);
        }
        
        public ref T AddComponent<T>(Entity entity, T t) where T : struct
        {
            ref var newT = ref world.GetPool<T>().Add(entity);
            newT = t;
            return ref newT;
        }
        
        public ref T AddComponent<T>(int e, T t) where T : struct
        {
            ref var newT = ref world.GetPool<T>().Add(e);
            newT = t;
            return ref newT;
        }
        
        public ref T GetComponent<T>(int e) where T : struct
        {
            return ref world.GetPool<T>().Get(e);
        }
        
        public ref T GetComponent<T>(Entity entity) where T : struct
        {
            return ref world.GetPool<T>().Get(entity);
        }
        
        public object GetComponent(Entity entity, Type type)
        {
            return world.GetPoolByType(type).GetRaw(entity);
        }
        
        public bool TryGetComponent<T>(Entity entity, out T component) where T : struct
        {
            component = default;
            if (!HasComponent<T>(entity))
                return false;
            component = GetComponent<T>(entity);
            return true;
        }
        
        public bool HasComponent<T>(int e) where T : struct
        {
            return world.GetPool<T>().Has(e);
        }

        public bool HasComponent<T>(Entity entity) where T : struct
        {
            return world.GetPool<T>().Has(entity);
        }
        
        public new EcsPool<T> GetComponents<T>() where T : struct
        {
            return world.GetPool<T>();
        }
        
        public EcsWorld.Mask GetFilter<T>() where T : struct
        {
            return world.Filter<T>();
        }

        public EcsFilter Filter<T1>() where T1 : struct
        {
            return world.Filter<T1>().End();
        }
        
        public EcsFilter Filter<T1, T2>() 
            where T1 : struct 
            where T2 : struct
        {
            return world.Filter<T1>().Inc<T2>().End();
        }
        
        public EcsFilter Filter<T1, T2, T3>() 
            where T1 : struct 
            where T2 : struct
            where T3 : struct
        {
            return world.Filter<T1>().Inc<T2>().Inc<T3>().End();
        }
        
        public EcsFilter Filter<T1, T2, T3, T4>() 
            where T1 : struct 
            where T2 : struct
            where T3 : struct
            where T4 : struct
        {
            return world.Filter<T1>().Inc<T2>().Inc<T3>().Inc<T4>().End();
        }

        private void Register(Component systemsParent, EcsSystems ecsSystems)
        {
            var systems = systemsParent.GetComponentsInChildren<IEcsSystem>();
            foreach (var system in systems)
            {
                if (system is BaseSystem baseSystem)
                {
                    baseSystem.world = this;
                }
                ecsSystems.Add(system);

                if (system is IEntityCreatedHandler entityCreatedHandler)
                {
                    entityCreatedHandlers.Add(entityCreatedHandler);
                }

                if (system is IEntityDestroyedHandler entityDestroyedHandler)
                {
                    entityDestroyedHandlers.Add(entityDestroyedHandler);
                }
            }
        }

        protected override void Awake()
        {
            base.Awake();
            Init();
        }

        public void Init() {
            
            if (initialized)
            {
                return;
            }
            
            world = new EcsWorld ();

            fixedUpdateSystems = new EcsSystems(world, sharedData);
            updateSystems = new EcsSystems(world, sharedData);
            lateUpdateSystems = new EcsSystems(world, sharedData);
            
            if (fixedUpdateParent != null)
            {
                Register(fixedUpdateParent, fixedUpdateSystems);
            }

            if (updateParent != null)
            {
                Register(updateParent, updateSystems);
            }

            if (lateUpdateParent != null)
            {
                Register(lateUpdateParent, lateUpdateSystems);
            }
            
#if UNITY_EDITOR
            if (Application.isPlaying && !disableLeoEcsDebug)
            {
                // add debug systems for custom worlds here, for example:
                // .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ("events"))
                updateSystems.Add(new EcsWorldDebugSystem());
            }
#endif

            fixedUpdateSystems.Inject();
            updateSystems.Inject();
            lateUpdateSystems.Inject();
            
            fixedUpdateSystems.Init ();
            updateSystems.Init();
            lateUpdateSystems.Init();

            initialized = true;
        }

        private void FixedUpdate()
        {
            // time.deltaTime = UnityEngine.Time.deltaTime;
            fixedUpdateSystems.Run ();
        }

        private void Update () {
            // time.deltaTime = UnityEngine.Time.deltaTime;
            updateSystems.Run ();
        }

        private void LateUpdate()
        {
            // time.deltaTime = UnityEngine.Time.deltaTime;
            lateUpdateSystems.Run ();
        }

        protected override void OnDestroy () {
            base.OnDestroy();
            
            if (fixedUpdateSystems != null) {
                fixedUpdateSystems.Destroy ();
                fixedUpdateSystems = null;
            }
        
            if (updateSystems != null) {
                updateSystems.Destroy ();
                updateSystems = null;
            }
        
            if (lateUpdateSystems != null) {
                lateUpdateSystems.Destroy ();
                lateUpdateSystems = null;
            }
        
            // destroy world.
            if (world != null) {
                world.Destroy ();
                world = null;
            }
        }

        private void OnEntityCreated(Entity entity)
        {
            foreach (var entityCreatedHandler in entityCreatedHandlers)
            {
                entityCreatedHandler.OnEntityCreated(this, entity);
            }

            if (onEntityCreated != null)
            {
                onEntityCreated(this, entity);
            }
        }
        
        private void OnEntityDestroyed(Entity entity)
        {
            foreach (var entityDestroyedHandler in entityDestroyedHandlers)
            {
                entityDestroyedHandler.OnEntityDestroyed(this, entity);
            }

            if (onEntityDestroyed != null)
            {
                onEntityDestroyed(this, entity);
            }
        }
    }
}
