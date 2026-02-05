using UnityEngine;

namespace Gemserk.Utilities
{
    public interface IDebugNamedObject
    {
        string GetObjectName();
    }
    
    public abstract class AutoNamedObject : MonoBehaviour
    {
        public virtual string GetObjectName()
        {
            return GetType().Name;
        }
        
#if UNITY_EDITOR 
        private void OnValidate()
        {
#if UNITY_2021_1_OR_NEWER
            if ( UnityEditor.SceneManagement.PrefabStageUtility.GetPrefabStage(gameObject) != null)
            {
                if (gameObject.transform.parent == null)
                {
                    return;
                }
            }
#elif UNITY_2019_1_OR_NEWER
            if (UnityEditor.Experimental.SceneManagement.PrefabStageUtility.GetPrefabStage(gameObject) != null)
            {
                if (gameObject.transform.parent == null)
                {
                    return;
                }
            }
#endif

            if (!gameObject.scene.IsValid())
            {
                return;
            }
            
            if (gameObject.GetComponents<MonoBehaviour>().Length == 1)
                gameObject.name = GetObjectName();
        }
#endif
    }
}