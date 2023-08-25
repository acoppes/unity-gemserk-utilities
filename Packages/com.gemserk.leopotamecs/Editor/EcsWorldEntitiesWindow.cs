using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Gemserk.Leopotam.Ecs.Systems;
using Leopotam.EcsLite;
using Leopotam.EcsLite.UnityEditor;
using NUnit.Framework;
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

        private bool sortedByName;
        
        private bool openAll;
        private bool closeAll;
        
        private Vector2 scrollPosition;
        private Entity selectedEntity = Entity.NullEntity;
        
        private Dictionary<Type, bool> foldouts = new Dictionary<Type, bool>();
        private static readonly Color SelectedEntityBackgroundColor = new Color(0.75f, 0.75f, 1f, 1f);

        private void OnEnable()
        {
            EcsWorldEntitiesWindowDebugSystem.windowOpenCount++;
        }

        private void OnDisable()
        {
            EcsWorldEntitiesWindowDebugSystem.windowOpenCount--;
        }

        void DrawComponents (EcsWorld world, Entity entity)
        {
            var count = world.GetComponents (entity.ecsEntity, ref _componentsCache);

            var componentsList = new List<object>();

            for (var i = 0; i < count; i++)
            {
                componentsList.Add(_componentsCache[i]);
            }
         
            if (sortedByName)
            {
                componentsList.Sort((o, o1) =>
                    string.Compare(o.GetType().Name, o1.GetType().Name, StringComparison.OrdinalIgnoreCase));
            }
            
            for (var i = 0; i < componentsList.Count; i++) {
                var component = componentsList[i];
                // _componentsCache[i] = null;
                var type = component.GetType ();

                foldouts.TryAdd(type, false);

                if (openAll)
                {
                    foldouts[type] = true;
                }

                if (closeAll)
                {
                    foldouts[type] = false;
                }
                
                GUILayout.BeginVertical (GUI.skin.box);
                var typeName = EditorExtensions.GetCleanGenericTypeName (type);
                var pool = world.GetPoolByType (type);

                var customInspector = EcsComponentInspectors.GetCustomInspector(type);

                var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);

                // special case for tag components
                if (fields.Length == 0)
                {
                    var labelStyle = new GUIStyle(GUI.skin.label)
                    {
                        alignment = TextAnchor.MiddleCenter
                    };
                    EditorGUILayout.LabelField(typeName, labelStyle);
                }
                else
                {
                    var previousBgColor = GUI.backgroundColor;

                    var buttonStyle = new GUIStyle(GUI.skin.button);
                    
                    // type was selected
                    if (foldouts[type])
                    {
                        var tempColor = previousBgColor;
                        tempColor.a = 0.75f;
                        GUI.backgroundColor = tempColor;

                        buttonStyle.fontStyle = FontStyle.Bold;
                    }
                    else
                    {
                        var tempColor = previousBgColor;
                        tempColor.a = 0.25f;
                        GUI.backgroundColor = tempColor;
                    }


                    
                    if (GUILayout.Button(typeName, buttonStyle))
                    {
                        foldouts[type] = !foldouts[type];
                    }

                    GUI.backgroundColor = previousBgColor;
                    
                    // foldouts[type] = EditorGUILayout.Foldout(foldouts[type], typeName);

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
                            foreach (var field in fields)
                            {
                                DrawTypeField(entity, component, pool, field);
                            }

                            EditorGUI.indentLevel = indent;
                        }
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

        private void OnInspectorUpdate()
        {
            if (Application.isPlaying)
            {
                Repaint();
            }
        }

        private void OnGUI()
        {
            if (!Application.isPlaying)
            {
                EditorGUILayout.LabelField("It only works when running.");
                selectedEntity = Entity.NullEntity;
               // Repaint();
                return;
            }
            
            var world = World.Instance;

            if (world == null)
            {
                EditorGUILayout.LabelField("No world found");
                selectedEntity = Entity.NullEntity;
                // Repaint();
                return;
            }
            
            var selectedEntityStyle = new GUIStyle(GUI.skin.button)
            {
                //fontStyle = FontStyle.Bold,
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

            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("-- TOOLBAR --", titleStyle);
            sortedByName = EditorGUILayout.Toggle("Sort Components By Name", sortedByName);
            openAll = false;
            closeAll = false;
            if (GUILayout.Button("Open all"))
            {
                openAll = true;
            }
            if (GUILayout.Button("Close all"))
            {
                closeAll = true;
            }
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.Separator();
            
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

                if (debug.isSingletonByName)
                {
                    entityName = $"{entityName} - <UNIQUE>";
                }

                var isSelected = entity == selectedEntity;

                var style = notSelectedEntityStyle;

                var previousBgColor = GUI.backgroundColor;
                
                if (isSelected)
                {
                    style = selectedEntityStyle;
                    GUI.backgroundColor = SelectedEntityBackgroundColor;
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
                else
                {
                    if (debug.selected)
                    {
                        selectedEntity = entity;
                        debug.selected = false;
                    }
                }
                
                GUI.backgroundColor = previousBgColor;
            }
            
            // THEN RENDER INFO WITH FILTERS AND STUFF
            
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
            
            if (!selectedEntity.Exists())
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
            
            // Repaint();
        }
    }
}