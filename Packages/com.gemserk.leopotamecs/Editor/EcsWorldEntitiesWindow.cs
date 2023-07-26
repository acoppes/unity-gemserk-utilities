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
        
        void DrawComponents (EcsWorld world, Entity entity) {
            var count = world.GetComponents (entity.ecsEntity, ref _componentsCache);
            for (var i = 0; i < count; i++) {
                var component = _componentsCache[i];
                _componentsCache[i] = null;
                var type = component.GetType ();
                GUILayout.BeginVertical (GUI.skin.box);
                var typeName = EditorExtensions.GetCleanGenericTypeName (type);
                var pool = world.GetPoolByType (type);
                var (rendered, changed, newValue) = EcsComponentInspectors.Render (typeName, type, component, null);
                if (!rendered) {
                    EditorGUILayout.LabelField (typeName, EditorStyles.boldLabel);
                    var indent = EditorGUI.indentLevel;
                    EditorGUI.indentLevel++;
                    foreach (var field in type.GetFields (BindingFlags.Instance | BindingFlags.Public)) {
                        DrawTypeField (entity, component, pool, field);
                    }
                    EditorGUI.indentLevel = indent;
                } else {
                    if (changed) {
                        // update value.
                        pool.SetRaw (entity.ecsEntity, newValue);
                    }
                }
                GUILayout.EndVertical ();
                EditorGUILayout.Space ();
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

                var entityName = string.IsNullOrEmpty(debug.name)
                    ? $"{entity.ToString()}"
                    : $"{entity.ToString()} - {debug.name}";
                
                debug.foldout = EditorGUILayout.Foldout(debug.foldout, entityName);
                if (debug.foldout)
                {
                    EditorGUI.indentLevel++;

                    DrawComponents(world.EcsWorld, entity);
                    
                    // other info
                    // EditorGUILayout.LabelField($"Total Components: {debug.componentTypeCount}");
                    //
                    // var types = debug.componentTypes;
                    // foreach (var componentType in types)
                    // {
                    //     if (componentType == null)
                    //     {
                    //         continue;
                    //     }
                    //
                    //     var component = world.GetComponent(entity, componentType);
                    //
                    //     if (componentType.IsAssignableFrom(typeof(ConfigurationComponent)))
                    //     {
                    //         DebugForComponents.DebugConfigurationComponent(componentType.Name, (ConfigurationComponent) component);
                    //     }
                    //     else
                    //     {
                    //         EditorGUILayout.LabelField(componentType.Name);
                    //         EditorGUI.indentLevel++;
                    //         EditorGUILayout.LabelField(component.ToString());
                    //         EditorGUI.indentLevel--;
                    //     }
                    //     
                    //
                    // }
                    //
                    // EditorGUI.BeginDisabledGroup(true);
                    // EditorGUI.EndDisabledGroup();
                    
                    EditorGUI.indentLevel--;
                }
                
            }
            
            // THEN RENDER INFO WITH FILTERS AND STUFF
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }
    }
}