using System;

namespace Gemserk.Triggers
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TriggerEditorAttribute : Attribute
    {
        public string editorName;
        public string tooltip;

        public TriggerEditorAttribute(string editorName)
        {
            this.editorName = editorName;
        }
    }
}