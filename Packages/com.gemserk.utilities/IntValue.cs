namespace Gemserk.Utilities
{
    public struct IntValue
    {
        private int internalValue;
        private int previousValue;

        public int value
        {
            get => internalValue;
            set
            {
                previousValue = internalValue;
                internalValue = value;
            }
        }

        public bool DidChange()
        {
            return previousValue != internalValue;
        }
        
        public static implicit operator int(IntValue intValue) => intValue.value;
        
        public static implicit operator IntValue(int value) => new ()
        {
            internalValue = value,
            previousValue = value
        };
    }
}