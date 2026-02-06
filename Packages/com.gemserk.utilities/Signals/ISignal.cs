namespace Gemserk.Utilities.Signals
{
    public interface ISignal
    {
        delegate void OnSignal(object userData);
        delegate void OnSignalMultiple(object userData, params object[] extraParameters);
        
        void Register(OnSignal signalHandler);
        void Unregister(OnSignal signalHandler);
        void Register(OnSignalMultiple signalHandler);
        void Unregister(OnSignalMultiple signalHandler);
        void Signal(object userData);
        void Signal(params object[] userData);
        void Clear();
    }
}