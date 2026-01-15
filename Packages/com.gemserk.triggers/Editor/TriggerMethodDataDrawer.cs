using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Gemserk.Triggers.Editor
{
	public interface ITriggerMethodDataEditorProvider {

		List<string> GetMethodNames(MethodInfo[] methods);

	}

	[CustomPropertyDrawer(typeof(TriggerMethodData))]
	public class TriggerMethodDataDrawer : PropertyDrawer, ITriggerMethodDataEditorProvider
	{
		public List<string> GetMethodNames (MethodInfo[] methods)
		{
			return methods
				// .Where(m => m.ReturnType == typeof(void))
				.Where (m => m.GetParameters ().Length == 0 
					|| TriggerMethodData.IsIntParameterMethod(m)
					|| TriggerMethodData.IsFloatParameterMethod(m)
					|| TriggerMethodData.IsStringParameterMethod(m)
					|| TriggerMethodData.IsObjectParameterMethod(m))
				.Select (m => m.Name).ToList ();
		}
		
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);
			EditorGUILayout.LabelField("Method Data");
			EditorGUI.indentLevel++;
			// var methodData = property.boxedValue as TriggerMethodData;
			//
			// Debug.Log(methodData.methodName);
			// Debug.Log(fieldInfo.Name);
			
			Draw(this, property);
			EditorGUI.indentLevel--;
			EditorGUI.EndProperty();
		}

		// public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		// {
		// 	var baseHeight = base.GetPropertyHeight(property, label);
		// 	return baseHeight * 3;
		// }

		private void Draw(ITriggerMethodDataEditorProvider provider, SerializedProperty methodProperty) // , UnityEngine.Object target)
		{
			var serializedObject = methodProperty.serializedObject;
			
			serializedObject.Update ();

			// CustomEditorUtils.DrawScriptField (target);

			EditorGUILayout.PropertyField(methodProperty.FindPropertyRelative("target"));
			
			// methodProperty.FindPropertyRelative("target").objectReferenceValue = EditorGUILayout.ObjectField ("Target", methodData.target, typeof(GameObject), true) as GameObject;
			// methodData.target = EditorGUILayout.ObjectField ("Target", methodData.target, typeof(GameObject), true) as GameObject;

			var methodData = methodProperty.boxedValue as TriggerMethodData;
			
			if (methodData.target) {
				Component[] allComponents = methodData.target.GetComponents<Component> ();

				var componentNameProperty = serializedObject.FindProperty ("methodData.componentName");

				if (CustomEditorUtils.DrawList ("Component", componentNameProperty, allComponents.Select (c => c.GetType().Name).ToList ())) {
					methodData.componentName = componentNameProperty.stringValue;
					serializedObject.ApplyModifiedProperties();
				}

				if (!string.IsNullOrEmpty (methodData.componentName)) {

					var component = methodData.target.GetComponent (methodData.componentName);

					if (component)
					{

						// var isAction = serializedObject.targetObject.GetType().Name.Contains("Action");
						
						var methodNames = 
							provider.GetMethodNames (component.GetType ().GetMethods (TriggerMethodData.DefaultBindingFlags));

						var methodNameProperty = serializedObject.FindProperty ("methodData.methodName");

						if (CustomEditorUtils.DrawList ("Method", methodNameProperty, methodNames)) {
							methodData.methodName = methodNameProperty.stringValue;
							serializedObject.ApplyModifiedProperties();
						}

						if (TriggerMethodData.IsIntParameterMethod (component, methodData.methodName)) {
							methodData.IntParameter = EditorGUILayout.IntField ("Int", methodData.IntParameter);
							serializedObject.ApplyModifiedProperties ();
						}

						if (TriggerMethodData.IsFloatParameterMethod (component, methodData.methodName)) {
							methodData.FloatParameter = EditorGUILayout.FloatField ("Float", methodData.FloatParameter);
							serializedObject.ApplyModifiedProperties ();
						}

						if (TriggerMethodData.IsStringParameterMethod (component, methodData.methodName)) {
							methodData.StringParameter = EditorGUILayout.TextField ("String", methodData.StringParameter);
							serializedObject.ApplyModifiedProperties ();
						}

						if (TriggerMethodData.IsObjectParameterMethod (component, methodData.methodName)) {
							methodData.ObjectParameter = EditorGUILayout.ObjectField ("Object", methodData.ObjectParameter, typeof(UnityEngine.Object), true);
							serializedObject.ApplyModifiedProperties ();
						}

					} else {
						componentNameProperty.stringValue = "";
						methodData.componentName = "";
					}
				}
			}
			
		}


	}
}