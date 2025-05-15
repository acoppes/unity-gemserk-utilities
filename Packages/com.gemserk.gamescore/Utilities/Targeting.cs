using Game.Components;
using Gemserk.Utilities;
using MyBox;
using UnityEngine;

namespace Game.Utilities
{
    public interface ITargeting
    {
        public TargetingFilter targetingFilter { get; }
    }
    
    public class Targeting : MonoBehaviour, ITargeting
    {
        [SerializeField]
        protected TargetingFilter targeting = new TargetingFilter()
        {
            // targetTypes = TargetType.Everything,
            playerAllianceType = PlayerAllianceType.Enemies,
            aliveType = HealthComponent.AliveType.Alive,
            distanceType = TargetingFilter.CheckDistanceType.Nothing,
            angle = new MinMaxFloat(0, 180),
            range = new MinMaxFloat(0, 1),
            sorter = null
        };

        public TargetingFilter targetingFilter
        {
            get
            {
                var filter = targeting;
                if (filter.targetTypeMask)
                {
                    filter.targetTypes = (TargetType) filter.targetTypeMask.GetInterface<ITargetTypeMask>().GetTargetTypeMask();
                }
                return filter;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.75f);
            Gizmos.DrawWireSphere(transform.position, targeting.range.Min);
            
            Gizmos.color = new Color(1f, 1f, 1f, 0.75f);
            Gizmos.DrawWireSphere(transform.position, targeting.range.Max);
        }
    }
}