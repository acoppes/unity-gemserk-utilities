using Gemserk.Leopotam.Ecs;using UnityEngine;

public class CustomUnitConfiguration : MonoBehaviour, IConfiguration
{
    public float speed;
    
    public void Configure(World world, Entity entity)
    {
        ref var m = ref world.GetComponent<MyMovementComponent>(entity);
        m.speed = 10;
    }
}