using Gemserk.Leopotam.Ecs;
using UnityEngine;

public class TimeToLiveDefinition : MonoBehaviour, IEntityDefinition
{
    public float ttl;
    
    public void Apply(World world, int entity)
    {
        world.AddComponent(entity, new TimeToLiveComponent
        {
            ttl = ttl
        });
    }
}