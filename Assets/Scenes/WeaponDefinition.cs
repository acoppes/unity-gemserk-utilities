using Gemserk.Leopotam.Ecs;
using UnityEngine;

public class WeaponDefinition : MonoBehaviour, IEntityDefinition
{
    public float cooldown;
    
    public void Apply(World world, int entity)
    {
        world.AddComponent(entity, new Weapon
        {
            cooldown = cooldown
        });
        
        world.AddComponent(entity, new TargetComponent
        {
            
        });
    }
}