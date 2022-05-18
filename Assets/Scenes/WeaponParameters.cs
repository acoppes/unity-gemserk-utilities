using Gemserk.Leopotam.Ecs;
using UnityEngine;

public class WeaponParameters : MonoBehaviour, IEntityInstanceParameter
{
    public float extraCooldown;
    
    public void Apply(World world, int entity)
    {
        ref var weapon = ref world.GetComponent<Weapon>(entity);
        weapon.cooldown += extraCooldown;
    }
}