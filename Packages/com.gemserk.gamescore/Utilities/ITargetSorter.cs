using System.Collections.Generic;

namespace Game.Utilities
{
    public interface ITargetSorter
    {
        void Sort(List<Target> targets, RuntimeTargetingParameters runtimeTargetingParameters);
    }
}