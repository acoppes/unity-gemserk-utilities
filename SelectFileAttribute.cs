using UnityEngine;

namespace Gemserk.BuildTools
{
    public class SelectFileAttribute : PropertyAttribute
    {
        public bool isFolder;

        public SelectFileAttribute(bool isFolder = false)
        {
            this.isFolder = isFolder;
        }
    }
}