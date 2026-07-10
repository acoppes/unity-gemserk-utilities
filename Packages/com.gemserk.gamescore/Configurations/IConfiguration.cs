namespace Game.Configurations
{
    public interface IConfiguration
    {
        bool Has(string key);
        
        // bool Get<T>(string key, out T value);

        T Get<T>(string key);

        void Set<T>(string key, T value);

        IConfiguration GetConfiguration(string key);

        IConfiguration[] GetConfigurationArray(string key);
    }
}