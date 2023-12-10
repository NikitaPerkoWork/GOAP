using System;
using CrashKonijn.Goap.Configs;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Classes.Builders
{
    public class TargetSensorBuilder
    {
        private readonly TargetKeyBuilder _targetKeyBuilder;
        private readonly TargetSensorConfig _config;

        public TargetSensorBuilder(Type type, TargetKeyBuilder targetKeyBuilder)
        {
            _targetKeyBuilder = targetKeyBuilder;
            _config = new TargetSensorConfig()
            {
                Name = type.Name,
                ClassType = type.AssemblyQualifiedName
            };
        }

        public TargetSensorBuilder SetTarget<TTarget>()
            where TTarget : ITargetKey
        {
            _config.Key = _targetKeyBuilder.GetKey<TTarget>();
            
            return this;
        }
        
        public ITargetSensorConfig Build()
        {
            return _config;
        }

        public static TargetSensorBuilder Create<TTargetSensor>(TargetKeyBuilder targetKeyBuilder) where TTargetSensor : ITargetSensor
        {
            return new TargetSensorBuilder(typeof(TTargetSensor), targetKeyBuilder);
        }
    }
}