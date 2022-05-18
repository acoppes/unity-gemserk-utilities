using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.UnityEditor;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{
    public class WorldSharedData
    {
        public object sharedData;
        
        public readonly IDictionary<string, int> singletonEntities = 
            new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
        
    }
    
    public class World : MonoBehaviour
    {
        public EcsWorld world;

        public readonly WorldSharedData sharedData = new WorldSharedData();

        private EcsSystems fixedUpdateSystems, updateSystems, lateUpdateSystems;

        private bool initialized;

        public Action<World, int> onEntityCreated, onEntityDestroyed;

        public int CreateEntity(IEntityDefinition definition, IEnumerable<IEntityInstanceParameter> parametersList = null)
        {
            var entity = world.NewEntity();
            
            AddComponent(entity, new EntityDefinitionComponent
            {
                definition = definition,
                parameters = parametersList
            });
            
            definition.Apply(this, entity);
            
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

        public void DestroyEntity(int entity)
        {
            OnEntityDestroyed(entity);
            world.DelEntity(entity);
        }

        public void AddComponent<T>(int entity) where T : struct
        {
            world.GetPool<T>().Add(entity);
        }
        
        public void AddComponent<T>(int entity, T t) where T : struct
        {
            ref var newT = ref world.GetPool<T>().Add(entity);
            newT = t;
        }
        
        public ref T GetComponent<T>(int entity) where T : struct
        {
            return ref world.GetPool<T>().Get(entity);
        }
        
        public new EcsPool<T> GetComponents<T>() where T : struct
        {
            return world.GetPool<T>();
        }
        
        public EcsWorld.Mask GetFilter<T>() where T : struct
        {
            return world.Filter<T>();
        }

        private void Register<T>(EcsSystems ecsSystems) where T: IEcsSystem
        {
            var systems = GetComponentsInChildren<T>();
            foreach (var system in systems)
            {
                if (system is BaseSystem baseSystem)
                {
                    baseSystem.world = this;
                }
                ecsSystems.Add(system);
            }
        }

        private void Awake()
        {
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
            
            Register<IFixedUpdateSystem>(fixedUpdateSystems);
            Register<IUpdateSystem>(updateSystems);
            Register<ILateUpdateSystem>(lateUpdateSystems);
            
#if UNITY_EDITOR
                // add debug systems for custom worlds here, for example:
                // .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ("events"))
                updateSystems.Add(new EcsWorldDebugSystem());
#endif

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

        private void OnDestroy () {
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

        private void OnEntityCreated(int entity)
        {
            onEntityCreated?.Invoke(this, entity);
        }
        
        private void OnEntityDestroyed(int entity)
        {
            onEntityDestroyed?.Invoke(this, entity);
        }
    }
}
