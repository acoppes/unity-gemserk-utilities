using System;
using MyBox;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{
    [Serializable]
    public class WorldReference
    {
        public enum Type
        {
            Default = 0,
            Name = 1,
            Tag = 2,
            SameScene = 3
        }

        public Type type;

        [ConditionalField(nameof(type), false, Type.Name)]
        public string name;
        
       // [Tag]
        [ConditionalField(nameof(type), false, Type.Tag)]
        public string tag;

        public World GetWorld(GameObject go = null)
        {
            if (type == Type.Default)
            {
                return World.Default;
            }
            
            if (type == Type.Name)
            {
                return World.GetByName(name);
            }

            if (type == Type.Tag)
            {
                return World.GetByTag(name);
            }

            if (type == Type.SameScene && go != null)
            {
                return World.GetByScene(go.scene);
            }

            return null;
        }
    }
}