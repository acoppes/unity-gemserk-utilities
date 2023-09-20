using System.Collections.Generic;
using System.Linq;
using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers;
using Gemserk.Triggers.Queries;
using Gemserk.Utilities;
using MyBox;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Triggers
{
    public class SpawnPackTriggerAction : WorldTriggerAction
    {
        [DisplayInspector]
        public Query query;
        
        [ConditionalField(nameof(query), true)]
        public TriggerTarget triggerTarget = new TriggerTarget();

        // spawnInstanceId of the pack instance
        [FormerlySerializedAs("identifier")] 
        public string spawnInstanceId;
        
        public Object spawnPack;
        
        [ConditionalField(nameof(spawnPack), true)]
        public TriggerTarget packId;

        public int count = 1;
        
        public override string GetObjectName()
        {
            string source;

            if (query != null)
            {
                source = query.ToString();
            }
            else
            {
                source = triggerTarget.ToString();
            }
            
            if (spawnPack != null)
            {
                return $"Spawn({spawnPack.name}, {source})";
            }
            
            return $"Spawn({packId}, {source})";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            List<IEntityDefinition> definitions = null;

            if (spawnPack != null)
            {
                definitions = spawnPack.GetInterface<ISpawnPack>().Definitions
                    .Select(o => o.GetInterface<IEntityDefinition>()).ToList();
                
            } else
            {
                var packDefinition = world.GetTriggerFirstEntity( null, packId, activator);
                // var packDefinition = world.GetFirstOrDefault(new EntityQuery(new NameParameter(packDefinitionId)));
                definitions = packDefinition.Get<PackComponent>().definitions;
            }
            
            var entities = new List<Entity>();
            world.GetTriggerTargetEntities(query, triggerTarget, activator, entities);
            
            foreach (var entity in entities)
            {
                ref var spawnerComponent = ref world.GetComponent<SpawnerComponent>(entity);
                for (int i = 0; i < count; i++)
                {
                    spawnerComponent.pending.Add(new SpawnPackData()
                    {
                        name = spawnInstanceId,
                        definitions = definitions
                    });
                }

            }
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}