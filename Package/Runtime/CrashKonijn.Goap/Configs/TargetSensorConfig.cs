using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Configs
{
    public class TargetSensorConfig<TSensor> : TargetSensorConfig
        where TSensor : ITargetSensor
    {
        public TargetSensorConfig()
        {
            Name = typeof(TSensor).Name;
            ClassType = typeof(TSensor).AssemblyQualifiedName;
        }
        
        public TargetSensorConfig(string name)
        {
            Name = name;
            ClassType = typeof(TSensor).AssemblyQualifiedName;
        }
    }

    public class TargetSensorConfig : ITargetSensorConfig
    {
        public string Name { get; set; }
        public string ClassType { get; set; }
        public ITargetKey Key { get; set; }
    }
}