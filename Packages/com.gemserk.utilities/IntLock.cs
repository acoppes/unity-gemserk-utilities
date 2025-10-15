using System;

namespace Gemserk.Utilities
{
    public struct IntLock
    {
        private int intLock;

        public bool value {
            get => intLock == 0;
            set {
                intLock = value ? intLock - 1 : intLock + 1;
                if (intLock < 0)
                    throw new Exception("Lock should never be negative");
            }
        }

        public bool locked
        {
            get => intLock > 0;
            set
            {
                intLock = value ? intLock + 1 : intLock - 1;
                if (intLock < 0)
                    throw new Exception("Lock should never be negative");
            }
        }

        public override string ToString()
        {
            return $"V:{value.ToString()}, L:{intLock}";
        }
    }
}