using UnityEngine.Assertions;

namespace Game.Utilities
{
    public struct MultipleStatesValue<T> where T : struct
    {
        private T[] values;
        private int currentIndex;

        private int previousIndex => currentIndex == 0 ? values.Length - 1 : currentIndex - 1;
        
        public MultipleStatesValue(T initial, int storedValues = 2)
        {
            Assert.IsTrue(storedValues > 1);
            
            values = new T[storedValues];
            for (var i = 0; i < storedValues; i++)
            {
                values[i] = initial;
            }
            currentIndex = 0;
        }

        public T value
        {
            get => values[currentIndex];
            set
            {
                values[previousIndex] = values[currentIndex];

                currentIndex = (currentIndex + 1) % values.Length;
                values[currentIndex] = value;
            }
        }

        public bool WasChanged()
        {
            var previousT = values[previousIndex];
            var currentT = values[currentIndex];
            return !previousT.Equals(currentT);
        }
    }
}