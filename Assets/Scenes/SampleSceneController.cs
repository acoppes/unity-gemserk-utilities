using Gemserk.Leopotam.Ecs;
using UnityEngine;

public class GameSharedData
{
    
}

public class SampleSceneController : MonoBehaviour
{
    public World world;

    
    // Start is called before the first frame update
    void Start()
    {
        world.sharedData = new GameSharedData();
        world.Init();

        // var entity = world.NewEntity();
        //
        // world.AddComponent(entity, new Weapon
        // {
        //     cooldown = 4,
        //     name = "WEAPON1"
        // });
    }
}
