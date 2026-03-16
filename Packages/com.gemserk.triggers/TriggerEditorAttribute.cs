using System;

namespace Gemserk.Triggers
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TriggerEditorAttribute : Attribute
    {
        public string editorName;
        public string tooltip;

        public TriggerEditorAttribute()
        {
            
        }

        public TriggerEditorAttribute(string editorName)
        {
            this.editorName = editorName;
        }
        
        public TriggerEditorAttribute(string editorName, string tooltip)
        {
            this.editorName = editorName;
            this.tooltip = tooltip;
        }
    }
}