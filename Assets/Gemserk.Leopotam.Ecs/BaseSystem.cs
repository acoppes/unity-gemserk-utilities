using System;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{
    public abstract class BaseSystem : MonoBehaviour
    {
        [NonSerialized]
        public World world;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (gameObject.GetComponents<Component>().Length == 2)
            {
                var updateType = string.Empty;

                switch (this)
                {
                    case IFixedUpdateSystem _:
                        updateType = "[FIXED UPDATE]";
                        break;
                    case IUpdateSystem _:
                        updateType = "[UPDATE]";
                        break;
                    case ILateUpdateSystem _:
                        updateType = "[LATE UPDATE]";
                        break;
                } 
                
                gameObject.name = $"{GetType().Name} - {updateType}";
            }
        }
#endif
    }
}