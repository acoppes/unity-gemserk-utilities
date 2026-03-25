using Gemserk.Triggers.Editor;
using Gemserk.Triggers.Queries;
using UnityEditor;
using UnityEngine;

namespace Gemserk.Triggers
{
    [InitializeOnLoad]
    public class QueryTriggerExtraActions
    {
        static QueryTriggerExtraActions()
        {
            TriggerSystemEditorWindow.extraActions.Add(("New Query", o =>
            {
                if (!o)
                {
                    return;
                }
                
                var queriesParent = o.transform.Find("Queries");
                if (!queriesParent)
                {
                    var queriesParentObject = new GameObject("Queries");
                    queriesParent = queriesParentObject.transform;
                    queriesParent.SetParent(o.transform, false);
                }

                var queryObject = new GameObject("Query");
                var query = queryObject.AddComponent<Query>();
                query.disableEditorAutoName = true;
                queryObject.transform.SetParent(queriesParent.transform, false);
                
                EditorGUIUtility.PingObject(queryObject);
            }));
        }
    }
}