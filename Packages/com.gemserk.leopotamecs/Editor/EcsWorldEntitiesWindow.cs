using Leopotam.EcsLite.Di;
using UnityEditor;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs.Editor
{
    public class EcsWorldEntitiesWindow : EditorWindow
    {
        [MenuItem("Window/Gemserk/ECS World Entities")]
        public static void ShowWindow()
        {
            EditorWindow wnd = GetWindow<EcsWorldEntitiesWindow>();
            wnd.titleContent = new GUIContent("Ecs World Entities");
        }
        
        private Vector2 scrollPosition;
        
        private void OnGUI()
        {
            if (!Application.isPlaying)
            {
                EditorGUILayout.LabelField("It only works when running.");
                return;
            }
            
            var world = World.Instance;

            if (world == null)
            {
                EditorGUILayout.LabelField("No world found");
                return;
            }
            
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, false, false);
            EditorGUILayout.BeginVertical();

            var filter = world.Filter<EcsWorldEntitiesDebugComponent>();
            var debugComponents = world.GetComponents<EcsWorldEntitiesDebugComponent>();
            
            foreach (var e in filter)
            {
                // COLLECT INFO HERE
                
                ref var debug = ref debugComponents.Get(e);
                // var debug = filter.Pools.Inc1.Get(e);
                // update debug stuff
                // debug.name = $"{}";
                var entity = world.GetEntity(e);
                
                debug.foldout = EditorGUILayout.Foldout(debug.foldout, $"{entity.ToString()} - {debug.name}");
                if (debug.foldout)
                {
                    EditorGUI.indentLevel++;
                    // other info
                    EditorGUILayout.LabelField($"Total Components: {debug.componentTypeCount}");

                    var types = debug.componentTypes;
                    foreach (var componentType in types)
                    {
                        if (componentType == null)
                        {
                            continue;
                        }
                        
                        var component = world.GetComponent(entity, componentType);
                        
                        EditorGUILayout.LabelField(componentType.Name);
                        EditorGUI.indentLevel++;
                        EditorGUILayout.LabelField(component.ToString());
                        EditorGUI.indentLevel--;
                    }
                    
                    EditorGUI.BeginDisabledGroup(true);

                    EditorGUI.EndDisabledGroup();
                    EditorGUI.indentLevel--;
                }
                
            }
            
            // THEN RENDER INFO WITH FILTERS AND STUFF
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }
    }
}