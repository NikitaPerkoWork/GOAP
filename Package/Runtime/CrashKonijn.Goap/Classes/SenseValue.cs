namespace CrashKonijn.Goap.Classes
{
    public struct SenseValue
    {
        private readonly int _value;
        
        public SenseValue(int value)
        {
            _value = value;
        }
        
        public SenseValue(bool value)
        {
            _value = value ? 1 : 0;
        }

        public static implicit operator int(SenseValue senseValue) => senseValue._value;

        public static implicit operator SenseValue(int value) => new(value);
        public static implicit operator SenseValue(bool value) => new(value);
    }
}