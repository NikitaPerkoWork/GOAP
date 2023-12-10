using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Configs
{
    public class WorldSensorConfig<TSensor> : WorldSensorConfig
        where TSensor : IWorldSensor
    {
        public WorldSensorConfig()
        {
            Name = typeof(TSensor).Name;
            ClassType = typeof(TSensor).AssemblyQualifiedName;
        }
        
        public WorldSensorConfig(string name)
        {
            Name = name;
            ClassType = typeof(TSensor).AssemblyQualifiedName;
        }
    }
    
    public class WorldSensorConfig : IWorldSensorConfig
    {
        public string Name { get; set; }
        public string ClassType { get; set; }
        public IWorldKey Key { get; set; }
    }
}