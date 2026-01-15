using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Gemserk.Triggers.Editor
{
	public static class CustomEditorUtils 
	{
		public static bool DrawList(string label, SerializedProperty property, List<string> optionNames)
		{
			string selectedObject = property.stringValue;

			optionNames.Insert (0, "None");

			if (string.IsNullOrEmpty(property.stringValue) && optionNames.Count > 0)
				property.stringValue = optionNames[0];

			int selected = 0;

			string[] options = new string[optionNames.Count];

			for (int i = 0; i < optionNames.Count; i++) {
				options[i] = optionNames[i];
				if (selectedObject.Equals(optionNames[i]))
					selected = i;
			}

			int newSelection = EditorGUILayout.Popup (label, selected, options);

			if (newSelection != selected) {

				if (newSelection > 0)
					property.stringValue = optionNames [newSelection];
				else
					property.stringValue = "";

				return true;
			}

			return false;
		}

		public static string DrawList(string label, bool allowNone, string selectedObject, List<string> optionNames)
		{
			if (allowNone) {
				optionNames.Insert (0, "None");

				if (string.IsNullOrEmpty(selectedObject))
					selectedObject = optionNames[0];
			}

			int selected = 0;

			string[] options = new string[optionNames.Count];

			for (int i = 0; i < optionNames.Count; i++) {
				options[i] = optionNames[i];
				if (selectedObject.Equals(optionNames[i]))
					selected = i;
			}

			int newSelection = EditorGUILayout.Popup (label, selected, options);

			if (newSelection != selected) {
				if (newSelection > 0 || !allowNone)
					return optionNames [newSelection];
				else
					return "";
			}

			return selectedObject;
		}

		public static bool DrawList(Rect position, string label, SerializedProperty property, List<string> optionNames)
		{
			string selectedObject = property.stringValue;

			optionNames.Insert (0, "None");

			if (string.IsNullOrEmpty(property.stringValue) && optionNames.Count > 0)
				property.stringValue = optionNames[0];

			int selected = 0;

			string[] options = new string[optionNames.Count];

			for (int i = 0; i < optionNames.Count; i++) {
				options[i] = optionNames[i];
				if (selectedObject.Equals(optionNames[i]))
					selected = i;
			}

			int newSelection = EditorGUI.Popup (position, label, selected, options);

			if (newSelection != selected) {

				if (newSelection > 0)
					property.stringValue = optionNames [newSelection];
				else
					property.stringValue = "";

				return true;
			}

			return false;
		}

		// public static void DrawScriptField(Object target)
		// {
		// 	DrawScriptField("Script", target);
		// }
		//
		// public static void DrawScriptField(string label, Object target)
		// {
		// 	var behaviour = target as MonoBehaviour;
		//
		// 	EditorGUI.BeginDisabledGroup(true);
		// 	EditorGUILayout.ObjectField(label, MonoScript.FromMonoBehaviour(behaviour), behaviour.GetType(), false);
		// 	EditorGUI.EndDisabledGroup();
		// }
	}
}
