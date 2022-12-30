using Gemserk.Leopotam.Ecs;
using UnityEngine;

[CreateAssetMenu(menuName = "Create ScriptableDefinition", fileName = "ScriptableDefinition", order = 0)]
public class ScriptableDefinition : ScriptableObject, IEntityDefinition
{
    public void Apply(World world, Entity entity)
    {
        
    }
}