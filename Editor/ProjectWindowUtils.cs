using System.Reflection;
using UnityEditor;

namespace Gemserk.Utilities.Editor
{
    public static class ProjectWindowUtils
    {
        public static string GetProjectWindowCurrentPath()
        {
            // TODO: check if project window is open?
            var projectWindowUtilType = typeof(ProjectWindowUtil);
            var getActiveFolderPath = projectWindowUtilType.GetMethod("GetActiveFolderPath", BindingFlags.Static | BindingFlags.NonPublic);
            var obj = getActiveFolderPath.Invoke(null, new object[0]);
            return obj.ToString();
        }
    }
}