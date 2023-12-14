using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using Demos.Complex.Interfaces;

namespace Demos.Complex.Sensors.World
{
    public class IsHoldingSensor<T> : LocalWorldSensorBase
        where T : IHoldable
    {
        public override SenseValue Sense(IMonoAgent agent)
        {
            return true;
        }
    }
}