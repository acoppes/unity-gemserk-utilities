﻿using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace Gemserk.Leopotam.Gameplay.Controllers
{
    public abstract class ControllerBase : MonoBehaviour, IController
    {
        protected World world;
        protected Entity entity;

        public void Bind(World world, Entity entity)
        {
            this.world = world;
            this.entity = entity;
        }
        
        public new ref T GetComponent<T>() where T : struct
        {
            return ref world.GetComponent<T>(entity);
        }
    }
}