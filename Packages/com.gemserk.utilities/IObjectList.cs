using System.Collections.Generic;
using UnityEngine;

namespace Gemserk.Utilities
{
    public interface IObjectList
    {
        T FindByName<T>(string name) where T : Object;

        void CollectByName<T>(string name, List<T> results) where T : Object;

        void Collect<T>(List<T> results) where T : Object;

        List<T> Get<T>() where T : Object;

        List<T> GetComponents<T>() where T : Component;
    }
}