using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Gemserk.Utilities;
using UnityEditor;

namespace Gemserk.Triggers.Editor
{
    public class TriggerSystemFilteredTypeCache
    {
        public List<TypeInfo> allActionTypes;
        public List<TypeInfo> allEventTypes;
        public List<TypeInfo> allConditionTypes;

        public List<TypeInfo> filteredActionTypes;
        public List<TypeInfo> filteredEventTypes;
        public List<TypeInfo> filteredConditionTypes;

        private string lastFilter = null;

        public void Init()
        {
            allActionTypes = PreProcess(TriggerEditorsTypeCaches.actionTypes);
            allEventTypes = PreProcess(TriggerEditorsTypeCaches.eventTypes);
            allConditionTypes = PreProcess(TriggerEditorsTypeCaches.conditionTypes);
        }

        public void Filter(string filter)
        {
            string[] searchTexts = null;
            if (!string.IsNullOrEmpty(filter))
            {
                searchTexts = StringUtilities.SplitSearchText(filter);
            }

            filteredActionTypes = FilterTypeList(searchTexts, allActionTypes);
            filteredEventTypes = FilterTypeList(searchTexts, allEventTypes);
            filteredConditionTypes = FilterTypeList(searchTexts, allConditionTypes);
        }

        private List<TypeInfo> FilterTypeList(string[] filterItems, List<TypeInfo> types)
        {
            // string[] searchTexts = null;
            // if (!string.IsNullOrEmpty(filter))
            // {
            //     searchTexts = StringUtilities.SplitSearchText(filter);
            // }
            
            if (filterItems != null)
            {
                return types.Where(info => StringUtilities.MatchAll(info.visualName, filterItems)).ToList();
            }

            return types.ToList();
        }

        private List<TypeInfo> PreProcess(TypeCache.TypeCollection typeCollection)
        {
            List<TypeInfo> infos = new();
            foreach (var type in typeCollection)
            {
                if (type.IsAbstract)
                    continue;

                if (type.GetCustomAttribute<ObsoleteAttribute>() != null)
                {
                    continue;
                }

                var buttonName = type.Name;
                var tooltip = string.Empty;
                
                var attributes = type.GetCustomAttributes(typeof(TriggerEditorAttribute)).ToList();

                if (attributes.Count > 0)
                {
                    var editorAttribute = attributes[0] as TriggerEditorAttribute;
                    buttonName = editorAttribute.editorName;
                    tooltip = editorAttribute.tooltip;
                }
                else
                {
                    buttonName = buttonName.Replace("TriggerCondition", "");
                    buttonName = buttonName.Replace("TriggerEvent", "");
                    buttonName = buttonName.Replace("TriggerAction", "");
                }

                var typeInfo = new TypeInfo
                {
                    visualName = buttonName,
                    type = type,
                    tooltip = tooltip
                };
                infos.Add(typeInfo);
            }

            infos.Sort((ti1, ti2) => string.CompareOrdinal(ti1.visualName, ti2.visualName));

            return infos;
        }
        
        
        public class TypeInfo
        {
            public string visualName;
            public Type type;
            public string tooltip;
        }
    }
}