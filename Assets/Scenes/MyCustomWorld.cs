using Leopotam.EcsLite;
using UnityEngine;

public class MyCustomWorld : MonoBehaviour
{
    private EcsWorld world;
    
    private EcsSystems fixedUpdateSystems, updateSystems, lateUpdateSystems;

    private void Awake () {
        // create ecs environment.
        world = new EcsWorld ();

        fixedUpdateSystems = new EcsSystems(world);
        updateSystems = new EcsSystems(world);
        lateUpdateSystems = new EcsSystems(world);

        var fixedUpdateSystemsList = GetComponentsInChildren<IFixedUpdateSystem>();
        foreach (var fixedUpdateSystem in fixedUpdateSystemsList)
        {
            fixedUpdateSystems.Add(fixedUpdateSystem);
        }
        
        fixedUpdateSystems.Init ();
    }
    
    private void FixedUpdate()
    {
        fixedUpdateSystems.Run ();
    }

    private void Update () {
        // process all dependent systems.
        updateSystems.Run ();
    }

    private void LateUpdate()
    {
        lateUpdateSystems.Run ();
    }

    private void OnDestroy () {
        // destroy systems logical group.
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
