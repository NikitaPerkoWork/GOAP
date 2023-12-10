using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Sensors
{
    public abstract class LocalWorldSensorBase : ILocalWorldSensor
    {
        public IWorldKey Key => Config.Key;

        public IWorldSensorConfig Config { get; private set; }
        public void SetConfig(IWorldSensorConfig config) => Config = config;

        public abstract void Created();
        public abstract void Update();
        public abstract SenseValue Sense(IMonoAgent agent, IComponentReference references);
    }
}