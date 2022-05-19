using Gemserk.Leopotam.Ecs;
using UnityEngine;

public class WeaponDefinition : MonoBehaviour, IEntityDefinition
{
    public float cooldown;

    public GameObject bulletDefinition;
    
    public void Apply(World world, int entity)
    {
        world.AddComponent(entity, new WeaponComponent
        {
            cooldown = cooldown,
            currentCooldown = 0,
            bulletDefinition = bulletDefinition.GetComponentInChildren<IEntityDefinition>()
        });
        
        world.AddComponent(entity, new TargetComponent
        {
            
        });
    }
}