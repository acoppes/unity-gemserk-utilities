namespace Game.Utilities
{
    public interface ITargetCustomFilter
    {
        /// <summary>
        /// Remove the targets that don't match the filter.
        /// </summary>
        /// <param name="target">The target to check.</param>
        /// <param name="runtimeTargetingParameters">Runtime data to consider when filtering.</param>
        bool Filter(Target target, RuntimeTargetingParameters runtimeTargetingParameters);
    }
}