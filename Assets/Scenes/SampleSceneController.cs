using Gemserk.Leopotam.Ecs;
using UnityEngine;

public class GameSharedData
{
    
}

public class SampleSceneController : MonoBehaviour
{
    public World world;

    public bool test;
    
    // Start is called before the first frame update
    void Start()
    {
        world.sharedData = new GameSharedData();
        world.Init();
        
        var entity = world.world.NewEntity();
        
        world.world.AddComponent(entity, new Weapon
        {
            cooldown = 4,
            name = "WEAPON1"
        });
    }

    // Update is called once per frame
    void Update()
    {
        test = !test;
    }
}
