using Game.Components;
using Gemserk.Leopotam.Ecs;using UnityEngine;

public class CustomUnitConfigurationScript : MonoBehaviour, IConfigurationScript
{
    public float speed;
    
    public void Configure(World world, Entity entity)
    {
        ref var m = ref world.GetComponent<MovementComponent>(entity);
        m.baseSpeed = speed;
        
    }
}