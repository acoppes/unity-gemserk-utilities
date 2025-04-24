using System.Collections.Generic;

namespace Game.Utilities
{
    public interface ITargetCustomFilter
    {
        /// <summary>
        /// Remove the targets that don't match the filter.
        /// </summary>
        /// <param name="targets">The targets to check.</param>
        /// <param name="runtimeTargetingParameters">Runtime data to consider when filtering.</param>
        void Filter(List<Target> targets, RuntimeTargetingParameters runtimeTargetingParameters);
    }
}