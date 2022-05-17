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
        var weapons = world.world.GetPool<Weapon>();
        ref var weapon = ref weapons.Add (entity);
        weapon.cooldown = 4;
        weapon.name = "WEAPON1";
    }

    // Update is called once per frame
    void Update()
    {
        test = !test;
    }
}
