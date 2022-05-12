using System.Collections;
using System.Collections.Generic;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using UnityEngine;

public class SampleSceneController : MonoBehaviour
{
    public World world;
    
    // Start is called before the first frame update
    void Start()
    {
        var entity = world.world.NewEntity();
        var weapons = world.world.GetPool<Weapon>();
        ref var weapon = ref weapons.Add (entity);
        weapon.cooldown = 10;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
