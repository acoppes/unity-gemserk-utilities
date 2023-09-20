using System;
using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers.Queries;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Screens
{
    public class PlayerPortrait : MonoBehaviour
    {
        [NonSerialized]
        public World world;

        public Query playerQuery;

        public CanvasGroup canvasGroup;

        public Image healthBarCurrent;

        public Text killCountText;

        private int cachedKillCount;

        public void LateUpdate()
        {
            var entity = world.GetFirstOrDefault(playerQuery.GetEntityQuery());
            
            if (entity == Entity.NullEntity)
            {
                return;
            }

            healthBarCurrent.fillAmount = 0;

            var isNullOrDeath = entity == Entity.NullEntity || !world.Exists(entity);
            
            canvasGroup.alpha = isNullOrDeath ? 0.25f : 1;

            if (!isNullOrDeath)
            {
                if (world.HasComponent<HealthComponent>(entity))
                {
                    var hitPoints = world.GetComponent<HealthComponent>(entity);
                    healthBarCurrent.fillAmount = (float)hitPoints.current / hitPoints.total;
                }
                
                if (killCountText != null && world.HasComponent<KillCountComponent>(entity))
                {
                    var killCountComponent = world.GetComponent<KillCountComponent>(entity);
                    if (killCountComponent.count != cachedKillCount)
                    {
                        killCountText.text = $"{killCountComponent.count}";
                        cachedKillCount = killCountComponent.count;
                    }
                }
            }
            
            // update bar, etc
        }
    }
}