namespace Gemserk.Utilities.Signals
{
    public interface ISignal
    {
        delegate void OnSignal(object userData);
        
        void Register(OnSignal signalHandler);
        void Unregister(OnSignal signalHandler);
        void Signal(object userData);
    }
}