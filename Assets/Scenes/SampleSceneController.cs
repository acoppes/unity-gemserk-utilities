using System;
using Gemserk.Leopotam.Ecs;
using UnityEngine;

public class GameSharedData
{
    
}

public class SampleSceneController : MonoBehaviour
{
    public World world;

    public GameObject entityDefinition;

    
    // Start is called before the first frame update
    void Start()
    {
        world.Init();

        world.sharedData.sharedData = new GameSharedData();
        
        // var entity = world.NewEntity();
        //
        // world.AddComponent(entity, new Weapon
        // {
        //     cooldown = 4,
        //     name = "WEAPON1"
        // });
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            world.CreateEntity(entityDefinition.GetComponentInChildren<IEntityDefinition>());
        }
    }
}
