using Leopotam.EcsLite;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{
    public class World : MonoBehaviour
    {
        public EcsWorld world;
    
        private EcsSystems fixedUpdateSystems, updateSystems, lateUpdateSystems;

        private void Register<T>(EcsSystems ecsSystems) where T: IEcsSystem
        {
            var systems = GetComponentsInChildren<T>();
            foreach (var system in systems)
            {
                ecsSystems.Add(system);
            }
        }

        private void Awake () {
            world = new EcsWorld ();

            fixedUpdateSystems = new EcsSystems(world);
            updateSystems = new EcsSystems(world);
            lateUpdateSystems = new EcsSystems(world);
            
            Register<IFixedUpdateSystem>(fixedUpdateSystems);
            Register<IUpdateSystem>(updateSystems);
            Register<ILateUpdateSystem>(lateUpdateSystems);

            fixedUpdateSystems.Init ();
            updateSystems.Init();
            lateUpdateSystems.Init();
        }
    
        private void FixedUpdate()
        {
            fixedUpdateSystems.Run ();
        }

        private void Update () {
            updateSystems.Run ();
        }

        private void LateUpdate()
        {
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
    }
}
