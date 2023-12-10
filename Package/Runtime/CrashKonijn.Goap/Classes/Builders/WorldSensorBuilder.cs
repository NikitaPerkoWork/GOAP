using System;
using CrashKonijn.Goap.Configs;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Classes.Builders
{
    public class WorldSensorBuilder
    {
        private readonly WorldKeyBuilder _worldKeyBuilder;
        private readonly WorldSensorConfig _config;

        public WorldSensorBuilder(Type type, WorldKeyBuilder worldKeyBuilder)
        {
            _worldKeyBuilder = worldKeyBuilder;
            _config = new WorldSensorConfig
            {
                Name = type.Name,
                ClassType = type.AssemblyQualifiedName
            };
        }
        
        public WorldSensorBuilder SetKey<TWorldKey>()
            where TWorldKey : IWorldKey
        {
            _config.Key = _worldKeyBuilder.GetKey<TWorldKey>();
            
            return this; 
        }

        public IWorldSensorConfig Build()
        {
            return _config;
        }

        public static WorldSensorBuilder Create<TWorldSensor>(WorldKeyBuilder worldKeyBuilder)
            where TWorldSensor : IWorldSensor
        {
            return new WorldSensorBuilder(typeof(TWorldSensor), worldKeyBuilder);
        }
    }
}