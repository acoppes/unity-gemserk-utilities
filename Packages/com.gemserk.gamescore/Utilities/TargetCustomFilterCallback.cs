using System;

namespace Game.Utilities
{
    public class TargetCustomFilterCallback : ITargetCustomFilter
    {
        private readonly Func<Target, RuntimeTargetingParameters, bool> callback;

        public TargetCustomFilterCallback(Func<Target, RuntimeTargetingParameters, bool> callback)
        {
            this.callback = callback;
        }

        public bool Filter(Target target, RuntimeTargetingParameters runtimeTargetingParameters)
        {
            return callback(target, runtimeTargetingParameters);
        }
    }
}