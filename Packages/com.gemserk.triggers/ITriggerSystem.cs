using System;

namespace Gemserk.Triggers
{
    public interface ITriggerSystem
    {
        void Execute();
    }

    public interface ITriggerErrorData
    {
        void LogError(Exception e);
    }
}