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
                
                var debug = debugComponents.Get(e);
                // var debug = filter.Pools.Inc1.Get(e);
                // update debug stuff
                // debug.name = $"{}";
                EditorGUILayout.LabelField(debug.name);
            }
            
            // THEN RENDER INFO WITH FILTERS AND STUFF
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }
    }
}