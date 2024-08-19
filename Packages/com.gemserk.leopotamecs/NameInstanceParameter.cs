using Gemserk.Utilities;
using MyBox;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{
    public class NameInstanceParameter : MonoBehaviour, IEntityInstanceParameter
    {
        public enum NamingType
        {
            String = 1,
            CopyFromGameObject = 2
        }

        [Separator("Identify")] 
        public NamingType namingType = NamingType.String;
        [ConditionalField(nameof(namingType), false, NamingType.String)]
        public string entityName;

        public bool singleton;
        [ConditionalField(nameof(namingType), false, NamingType.String)]
        public bool disableSpawnNameOverride;

        public void Apply(World world, Entity entity)
        {
            if (!entity.Has<NameComponent>())
            {
                world.AddComponent(entity, new NameComponent());
            }
            
            ref var nameComponent = ref entity.Get<NameComponent>();
            nameComponent.name = namingType == NamingType.String ? entityName : gameObject.name;
            nameComponent.singleton = singleton;
        }
        
        private void OnValidate()
        {
            if (!gameObject.IsSafeToModifyName() || disableSpawnNameOverride)
                return;

            if (namingType == NamingType.String)
            {
                if (!string.IsNullOrEmpty(entityName))
                {
                    gameObject.name = $"Spawn({entityName})";
                }
                else
                {
                    gameObject.name = "Spawn()"; 
                }
            }
        }
    }
}