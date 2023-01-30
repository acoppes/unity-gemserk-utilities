namespace Gemserk.Utilities
{
    public interface IFuture
    {
        public enum State
        {
            Processing,
            Completed,
            Cancelled
        }

        State GetState();

        object GetValue();
        
        T GetValueAs<T>() where T : class;
    }
    
    public class Future : IFuture
    {
        public object value;
        public IFuture.State state;
        
        public IFuture.State GetState()
        {
            return state;
        }

        public object GetValue()
        {
            return value;
        }

        public T GetValueAs<T>() where T : class
        {
            return value as T;
        }
    }
}