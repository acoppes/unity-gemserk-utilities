using System;
using System.Collections.Generic;
using System.Reflection;
using Leopotam.EcsLite;
using Leopotam.EcsLite.UnityEditor;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gemserk.Leopotam.Ecs.Editor
{
    static class EcsComponentInspectors {
        static readonly Dictionary<Type, IEcsComponentInspector> Inspectors = new Dictionary<Type, IEcsComponentInspector> ();

        static EcsComponentInspectors () {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies ()) {
                foreach (var type in assembly.GetTypes ()) {
                    if (typeof (IEcsComponentInspector).IsAssignableFrom (type) && !type.IsInterface && !type.IsAbstract) {
                        if (Activator.CreateInstance (type) is IEcsComponentInspector inspector) {
                            var componentType = inspector.GetFieldType ();
                            if (!Inspectors.TryGetValue (componentType, out var prevInspector)
                                || inspector.GetPriority () > prevInspector.GetPriority ()) {
                                Inspectors[componentType] = inspector;
                            }
                        }
                    }
                }
            }
        }
        
        public static IEcsComponentInspector GetCustomInspector(Type type)
        {
            if (Inspectors.TryGetValue(type, out var inspector))
            {
                return inspector;
            }
            return null;
        }

        public static (bool, bool, object) Render (string label, Type type, object value, EcsEntityDebugView debugView) {
            if (Inspectors.TryGetValue (type, out var inspector)) {
                var (changed, newValue) = inspector.OnGui (label, value, debugView);
                return (true, changed, newValue);
            }
            return (false, false, null);
        }

        public static (bool, object) RenderEnum (string label, object value, bool isFlags) {
            var enumValue = (Enum) value;
            Enum newValue;
            if (isFlags) {
                newValue = EditorGUILayout.EnumFlagsField (label, enumValue);
            } else {
                newValue = EditorGUILayout.EnumPopup (label, enumValue);
            }
            if (Equals (newValue, value)) { return (default, default); }
            return (true, newValue);
        }
    }
    
    public class EcsWorldEntitiesWindow : EditorWindow
    {
        const int MaxFieldToStringLength = 128;

        static object[] _componentsCache = new object[32];

        [MenuItem("Window/Gemserk/ECS World Entities")]
        public static void ShowWindow()
        {
            EditorWindow wnd = GetWindow<EcsWorldEntitiesWindow>();
            wnd.titleContent = new GUIContent("Ecs World Entities");
        }
        
        private Vector2 scrollPosition;
        private Entity selectedEntity = Entity.NullEntity;
        
        private Dictionary<Type, bool> foldouts = new Dictionary<Type, bool>();
        
        void DrawComponents (EcsWorld world, Entity entity) {
            var count = world.GetComponents (entity.ecsEntity, ref _componentsCache);
            for (var i = 0; i < count; i++) {
                var component = _componentsCache[i];
                _componentsCache[i] = null;
                var type = component.GetType ();

                foldouts.TryAdd(type, false);

                GUILayout.BeginVertical (GUI.skin.box);
                var typeName = EditorExtensions.GetCleanGenericTypeName (type);
                var pool = world.GetPoolByType (type);

                var customInspector = EcsComponentInspectors.GetCustomInspector(type);

                foldouts[type] = EditorGUILayout.Foldout(foldouts[type], typeName);

                if (foldouts[type])
                {
                    if (customInspector != null)
                    {
                        var (changed, newValue) = customInspector.OnGui(typeName, component, null);

                        if (changed)
                        {
                            // update value.
                            pool.SetRaw(entity.ecsEntity, newValue);
                        }
                    }
                    else
                    {
                        var indent = EditorGUI.indentLevel;
                        EditorGUI.indentLevel++;
                        foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public))
                        {
                            DrawTypeField(entity, component, pool, field);
                        }

                        EditorGUI.indentLevel = indent;
                    }
                }

                GUILayout.EndVertical ();
                EditorGUILayout.Space (2f, true);
            }
        }

        void DrawTypeField (Entity entity, object component, IEcsPool pool, FieldInfo field) {
            var fieldValue = field.GetValue (component);
            var fieldType = field.FieldType;
            var (rendered, changed, newValue) = EcsComponentInspectors.Render (field.Name, fieldType, fieldValue, null);
            if (!rendered) {
                if (fieldType == typeof (Object) || fieldType.IsSubclassOf (typeof (Object))) {
                    GUILayout.BeginHorizontal ();
                    EditorGUILayout.LabelField (field.Name, GUILayout.MaxWidth (EditorGUIUtility.labelWidth - 16));
                    var newObjValue = EditorGUILayout.ObjectField (fieldValue as Object, fieldType, true);
                    if (newObjValue != (Object) fieldValue) {
                        field.SetValue (component, newObjValue);
                        pool.SetRaw (entity.ecsEntity, component);
                    }
                    GUILayout.EndHorizontal ();
                    return;
                }
                if (fieldType.IsEnum) {
                    var isFlags = Attribute.IsDefined (fieldType, typeof (FlagsAttribute));
                    var (enumChanged, enumNewValue) = EcsComponentInspectors.RenderEnum (field.Name, fieldValue, isFlags);
                    if (enumChanged) {
                        field.SetValue (component, enumNewValue);
                        pool.SetRaw (entity.ecsEntity, component);
                    }
                    return;
                }
                var strVal = fieldValue != null ? string.Format (System.Globalization.CultureInfo.InvariantCulture, "{0}", fieldValue) : "null";
                if (strVal.Length > MaxFieldToStringLength) {
                    strVal = strVal.Substring (0, MaxFieldToStringLength);
                }
                GUILayout.BeginHorizontal ();
                EditorGUILayout.PrefixLabel (field.Name);
                EditorGUILayout.SelectableLabel (strVal, GUILayout.MaxHeight (EditorGUIUtility.singleLineHeight));
                GUILayout.EndHorizontal ();
            } else {
                if (changed) {
                    // update value.
                    field.SetValue (component, newValue);
                    pool.SetRaw (entity.ecsEntity, component);
                }
            }
        }
        
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
            
            var selectedEntityStyle = new GUIStyle(GUI.skin.button)
            {
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleLeft
            };

            var notSelectedEntityStyle = new GUIStyle(GUI.skin.button)
            {
                fontStyle = FontStyle.Normal,
                alignment = TextAnchor.MiddleLeft
            };

            var titleStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter
            };

            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.BeginVertical(GUILayout.MinWidth(250), GUILayout.ExpandWidth(true));
            EditorGUILayout.LabelField("-- ENTITIES --", titleStyle);
            EditorGUILayout.Separator();

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, false, false);
            
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

                var entityName = string.IsNullOrEmpty(debug.name)
                    ? $"{entity.ToString()}"
                    : $"{entity.ToString()} - {debug.name}";

                var isSelected = entity == selectedEntity;

                var style = notSelectedEntityStyle;

                if (isSelected)
                {
                    style = selectedEntityStyle;
                }
                
                if (GUILayout.Button(entityName, style))
                {
                    if (isSelected)
                    {
                        selectedEntity = Entity.NullEntity;
                    }
                    else
                    {
                        selectedEntity = entity;
                    }
                }
            }
            
            // THEN RENDER INFO WITH FILTERS AND STUFF
            
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
            
            if (selectedEntity == Entity.NullEntity)
            {
                EditorGUILayout.LabelField("-- SELECT ENTITY --", titleStyle);
                EditorGUILayout.Separator();
            }
            else
            {
                ref var debug = ref debugComponents.Get(selectedEntity.ecsEntity);

                if (string.IsNullOrEmpty(debug.name))
                {
                    EditorGUILayout.LabelField($"-- id: {selectedEntity.ecsEntity}, gen: {selectedEntity.ecsGeneration} --", titleStyle);
                } else
                {
                    EditorGUILayout.LabelField(
                        $"-- id: {selectedEntity.ecsEntity}, gen: {selectedEntity.ecsGeneration}, name: {debug.name} --",
                        titleStyle);
                }
                
                EditorGUILayout.Separator();

                debug.scrollPosition = EditorGUILayout.BeginScrollView(debug.scrollPosition, false, false);
                DrawComponents(world.EcsWorld, selectedEntity);
                EditorGUILayout.EndScrollView();
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
    }
}