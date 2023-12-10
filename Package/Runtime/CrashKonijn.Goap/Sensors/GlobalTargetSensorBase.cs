using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Sensors
{
    public abstract class GlobalTargetSensorBase : IGlobalTargetSensor
    {
        public ITargetKey Key => Config.Key;
        
        public ITargetSensorConfig Config { get; private set; }
        public void SetConfig(ITargetSensorConfig config) => Config = config;

        public abstract void Created();
        public abstract ITarget Sense();
    }
}