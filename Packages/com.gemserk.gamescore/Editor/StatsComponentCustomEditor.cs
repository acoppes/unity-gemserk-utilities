using System.Collections.Generic;
using Game.Components;
using UnityEditor;

namespace Game.Editor
{
    [CustomEditor(typeof(StatsComponentDefinition))]
    public class StatsComponentCustomEditor : UnityEditor.Editor
    {
        private bool foldout = true;

        public override void OnInspectorGUI()
        {
            var statsComponentDef = target as StatsComponentDefinition;

            if (statsComponentDef)
            {
                if (statsComponentDef.statDefinitions.Count < RegisteredStats.TotalStats)
                {
                    var newValues = new List<StatsComponentDefinition.StatDefinition>();
                    for (int i = 0; i < RegisteredStats.TotalStats; i++)
                    {
                        if (i < statsComponentDef.statDefinitions.Count)
                        {
                            newValues.Add(statsComponentDef.statDefinitions[i]);
                        }
                        else
                        {
                            newValues.Add(new StatsComponentDefinition.StatDefinition()
                            {
                                // type = i,
                                value = 0
                            });
                        }
                    }

                    statsComponentDef.statDefinitions = newValues;
                }

                foldout = EditorGUILayout.Foldout(foldout, "Stats");

                if (foldout)
                {
                    EditorGUI.indentLevel++;
                    for (int i = 0; i < RegisteredStats.TotalStats; i++)
                    {
                        var name = RegisteredStats.StatToName(i);
                        name = name.Replace("Stat_", "");
                        EditorGUI.BeginChangeCheck();
                        statsComponentDef.statDefinitions[i].value =
                            EditorGUILayout.FloatField(name, statsComponentDef.statDefinitions[i].value);
                        if (EditorGUI.EndChangeCheck())
                        {
                            EditorUtility.SetDirty(target);
                        }
                    }

                    EditorGUI.indentLevel--;
                }
            }

            // DrawDefaultInspector();
        }
    }
}