using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Extensions;
using UnityEngine;

public class WeaponDefinition : MonoBehaviour, IEntityDefinition
{
    public float cooldown;
    
    public void Apply(World world, int entity)
    {
        world.AddComponent(entity, new Weapon
        {
            name = gameObject.name,
            cooldown = cooldown
        });
        
        world.AddComponent(entity, new TargetComponent
        {
            
        });
    }
}