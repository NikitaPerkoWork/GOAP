using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;

namespace CrashKonijn.Goap.UnitTests.Data
{
    public class LocalWorldSensor : LocalWorldSensorBase
    {
        public override SenseValue Sense(IMonoAgent agent)
        {
            return default;
        }
    }
}