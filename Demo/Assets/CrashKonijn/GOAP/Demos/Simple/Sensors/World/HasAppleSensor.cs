using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;

namespace Demos.Simple.Sensors.World
{
    public class HasAppleSensor : LocalWorldSensorBase
    {
        public override SenseValue Sense(IMonoAgent agent)
        {
            return true;
        }
    }
}