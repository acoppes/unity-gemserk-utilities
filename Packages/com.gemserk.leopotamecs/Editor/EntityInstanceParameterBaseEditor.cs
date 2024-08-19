using Gemserk.Utilities.Editor;
using UnityEditor;

namespace Gemserk.Leopotam.Ecs.Editor
{
    [CustomEditor(typeof(EntityInstanceParameterBase), true)]
    public class EntityInstanceParameterBaseEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.DrawInspectorExcept(new []{ "m_Script" });
        }
    }
}