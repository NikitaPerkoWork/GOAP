using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Sensors
{
    public abstract class GlobalWorldSensorBase : IGlobalWorldSensor
    {
        public IWorldKey Key => Config.Key;

        public IWorldSensorConfig Config { get; private set; }
        public void SetConfig(IWorldSensorConfig config) => Config = config;

        public abstract void Created();
        public abstract SenseValue Sense();
    }
}