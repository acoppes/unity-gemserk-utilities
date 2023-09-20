using System.Collections.Generic;
using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace Game.Components
{
    public struct SpawnPackData
    {
        public string name;
        public List<IEntityDefinition> definitions;
    }
    
    public interface ISpawnPack
    {
        List<Object> Definitions { get; }
    }
    
    public class SpawnPack : MonoBehaviour, ISpawnPack
    {
        [SerializeField]
        private List<Object> definitions;
        
        public List<Object> Definitions => definitions;
    }
}