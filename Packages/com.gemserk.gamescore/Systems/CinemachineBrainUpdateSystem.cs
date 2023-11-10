﻿using Unity.Cinemachine;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using UnityEngine;

namespace Game.Systems
{
    public class CinemachineBrainUpdateSystem : BaseSystem, IEcsRunSystem, IEcsInitSystem
    {
        private CinemachineBrain brain;
        
        public void Init(EcsSystems systems)
        {
            if (Camera.main != null)
            {
                brain = Camera.main.GetComponent<CinemachineBrain>();
            }
        }
        
        public void Run(EcsSystems systems)
        {
            if (brain != null && brain.UpdateMethod == CinemachineBrain.UpdateMethods.ManualUpdate)
            {
                brain.ManualUpdate();
            }
        }


    }
}