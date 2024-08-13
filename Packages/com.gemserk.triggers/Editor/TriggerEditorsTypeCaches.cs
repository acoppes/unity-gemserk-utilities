using UnityEditor;

namespace Gemserk.Triggers.Editor
{
    [InitializeOnLoad]
    public static class TriggerEditorsTypeCaches
    {
        public static TypeCache.TypeCollection eventTypes;
        public static TypeCache.TypeCollection conditionTypes;
        public static TypeCache.TypeCollection actionTypes;

        static TriggerEditorsTypeCaches()
        {
            eventTypes = TypeCache.GetTypesDerivedFrom<TriggerEvent>();
            conditionTypes = TypeCache.GetTypesDerivedFrom<TriggerCondition>();
            actionTypes = TypeCache.GetTypesDerivedFrom<TriggerAction>();
        }
    }
}