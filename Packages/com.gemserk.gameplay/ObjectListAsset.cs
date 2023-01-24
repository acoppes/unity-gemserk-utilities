using System.Collections.Generic;
using UnityEngine;

namespace Gemserk.Gameplay
{
    [CreateAssetMenu(menuName = "Gemserk/Object List", fileName = "ObjectListAsset", order = 0)]
    public class ObjectListAsset : ScriptableObject
    {
        public string path;
        
        public List<Object> assets = new List<Object>();
    }
}