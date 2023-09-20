using Game.Utilities;
using UnityEngine;

namespace Game.Components
{
    public interface ITargeting
    {
        public TargetingFilter targetingFilter { get; }
    }
    
    public class Targeting : MonoBehaviour, ITargeting
    {
        public TargetingFilter targeting;

        public TargetingFilter targetingFilter => targeting;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.75f);
            Gizmos.DrawWireSphere(transform.position, targeting.range.Min);
            
            Gizmos.color = new Color(1f, 1f, 1f, 0.75f);
            Gizmos.DrawWireSphere(transform.position, targeting.range.Max);
        }
    }
}