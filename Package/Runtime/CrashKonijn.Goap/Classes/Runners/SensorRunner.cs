using System.Collections.Generic;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Classes.Runners
{
    public class SensorRunner : ISensorRunner
    {
        private readonly HashSet<ILocalWorldSensor> _localWorldSensors = new();
        private readonly HashSet<IGlobalWorldSensor> _globalWorldSensors = new();
        private readonly HashSet<ILocalTargetSensor> _localTargetSensors = new();
        private readonly HashSet<IGlobalTargetSensor> _globalTargetSensors = new();
        
        // GC caches
        private LocalWorldData _localWorldData;
        private readonly GlobalWorldData _worldData = new();

        public SensorRunner(IEnumerable<IWorldSensor> worldSensors, IEnumerable<ITargetSensor> targetSensors)
        {
            foreach (var worldSensor in worldSensors)
            {
                switch (worldSensor)
                {
                    case ILocalWorldSensor localSensor:
                        _localWorldSensors.Add(localSensor);
                        break;
                    case IGlobalWorldSensor globalSensor:
                        _globalWorldSensors.Add(globalSensor);
                        break;
                }
            }

            foreach (var targetSensor in targetSensors)
            {
                switch (targetSensor)
                {
                    case ILocalTargetSensor localSensor:
                        _localTargetSensors.Add(localSensor);
                        break;
                    case IGlobalTargetSensor globalSensor:
                        _globalTargetSensors.Add(globalSensor);
                        break;
                }
            }
        }

        public void Update()
        {
            foreach (var localWorldSensor in _localWorldSensors)
            {
                localWorldSensor.Update();
            }

            foreach (var localTargetSensor in _localTargetSensors)
            {
                localTargetSensor.Update();
            }
        }

        public GlobalWorldData SenseGlobal()
        {
            foreach (var globalWorldSensor in _globalWorldSensors)
            {
                _worldData.SetState(globalWorldSensor.Key, globalWorldSensor.Sense());
            }
            
            foreach (var globalTargetSensor in _globalTargetSensors)
            {
                _worldData.SetTarget(globalTargetSensor.Key, globalTargetSensor.Sense());
            }

            return _worldData;
        }

        public LocalWorldData SenseLocal(GlobalWorldData worldData, IMonoAgent agent)
        {
            _localWorldData = (LocalWorldData) agent.WorldData;

            _localWorldData.Apply(worldData);
            
            foreach (var localWorldSensor in _localWorldSensors)
            {
                _localWorldData.SetState(localWorldSensor.Key, localWorldSensor.Sense(agent));
            }
            
            foreach (var localTargetSensor in _localTargetSensors)
            {
                _localWorldData.SetTarget(localTargetSensor.Key, localTargetSensor.Sense(agent));
            }
            
            return _localWorldData;
        }
    }
}