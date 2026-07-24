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
            angleType = TargetingFilter.CheckDistanceType.PlaneXZ,
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

            var p0 = transform.position;
            
            if (targeting.angle.Max < 180)
            {
                Gizmos.color = new Color(1f, 0.25f, 0.25f, 0.75f);
                
                var maxAngleRight = Vector2.right.Rotate(-targeting.angle.Max * Mathf.Deg2Rad) * targeting.range.Max;
                var maxAngleLeft = Vector2.right.Rotate(targeting.angle.Max * Mathf.Deg2Rad) * targeting.range.Max;
                
                Gizmos.DrawLine(p0, transform.position + (Vector3) maxAngleRight);
                Gizmos.DrawLine(p0, transform.position + (Vector3) maxAngleLeft);
            }
            
            if (targeting.angle.Min > 0)
            {
                Gizmos.color = new Color(1f, 1f, 0.25f, 0.75f);
                
                var minAngleRight = Vector2.right.Rotate(-targeting.angle.Min * Mathf.Deg2Rad) * targeting.range.Max;
                var minAngleLeft = Vector2.right.Rotate(targeting.angle.Min * Mathf.Deg2Rad) * targeting.range.Max;
              
                Gizmos.DrawLine(p0, transform.position + (Vector3) minAngleRight);
                Gizmos.DrawLine(p0, transform.position + (Vector3) minAngleLeft);
            }
        }
    }
}