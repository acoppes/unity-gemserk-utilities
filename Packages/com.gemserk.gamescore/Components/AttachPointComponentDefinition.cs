using System;
using System.Collections.Generic;
using Gemserk.Leopotam.Ecs;
using UnityEngine;
using Vertx.Debugging;

namespace Game.Components
{
    public class AttachPoint
    {
        public Vector3 entityPosition;
        public Vector3 localPosition;

        public Vector3 position => entityPosition + localPosition;
    }
    
    public struct AttachPointsComponent : IEntityComponent
    {
        public Dictionary<string, AttachPoint> attachPoints;

        public AttachPoint Get(string name)
        {
            return attachPoints[name];
        }
    }
    
    public class AttachPointComponentDefinition : ComponentDefinitionBase
    {
        [Serializable]
        public class AttachDefinition
        {
            public string name;
            public Vector3 position;
        }

        public List<AttachDefinition> attachPointDefinitions;

        public override string GetComponentName()
        {
            return nameof(AttachPointsComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            var attachPoints = new Dictionary<string, AttachPoint>(StringComparer.OrdinalIgnoreCase);

            foreach (var attachPointDefinition in attachPointDefinitions)
            {
                attachPoints[attachPointDefinition.name] = new AttachPoint()
                {
                    localPosition = attachPointDefinition.position
                };
            }
            
            world.AddComponent(entity, new AttachPointsComponent()
            {
                attachPoints = attachPoints
            });
        }

        private void OnDrawGizmosSelected()
        {
            if (attachPointDefinitions != null)
            {
                foreach (var attachPointDefinition in attachPointDefinitions)
                {
                    Gizmos.DrawWireSphere(transform.position + attachPointDefinition.position, 0.025f);
                    D.raw(new Shape.Text(transform.position + attachPointDefinition.position + new Vector3(0.05f, 0, 0), attachPointDefinition.name));
                }
            }
        }
    }
}