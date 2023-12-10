using System.Collections.Generic;
using CrashKonijn.Goap.Classes.Runners;
using CrashKonijn.Goap.Classes.Validators;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Exceptions;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Resolvers;
using UnityEngine;

namespace CrashKonijn.Goap.Classes
{
    public class GoapSetFactory
    {
        private readonly IGoapConfig _goapConfig;
        private readonly ClassResolver _classResolver = new();
        private readonly IGoapSetConfigValidatorRunner _goapSetConfigValidatorRunner = new GoapSetConfigValidatorRunner();

        public GoapSetFactory(IGoapConfig goapConfig)
        {
            _goapConfig = goapConfig;
        }
        
        public GoapSet Create(IGoapSetConfig config)
        {
            Validate(config);
            
            var sensorRunner = CreateSensorRunner(config);

            return new GoapSet(
                id: config.Name,
                config: _goapConfig,
                actions: GetActions(config),
                goals: GetGoals(config),
                sensorRunner: sensorRunner,
                debugger: GetDebugger(config)
            );
        }
        
        private void Validate(IGoapSetConfig config)
        {
            var results = _goapSetConfigValidatorRunner.Validate(config);

            foreach (var error in results.GetErrors())
            {
                Debug.LogError(error);
            }
            
            foreach (var warning in results.GetWarnings())
            {
                Debug.LogWarning(warning);
            }
            
            if (results.HasErrors())
            {
                throw new GoapException($"GoapSetConfig has errors: {config.Name}");
            }
        }
        
        private SensorRunner CreateSensorRunner(IGoapSetConfig config)
        {
            return new SensorRunner(GetWorldSensors(config), GetTargetSensors(config));
        }
        
        private List<IActionBase> GetActions(IGoapSetConfig config)
        {
            var actions = _classResolver.Load<IActionBase, IActionConfig>(config.Actions);
            var injector = _goapConfig.GoapInjector;
            
            actions.ForEach(x =>
            {
                injector.Inject(x);
                x.Created();
            });

            return actions;
        }
        
        private List<IGoalBase> GetGoals(IGoapSetConfig config)
        {
            var goals = _classResolver.Load<IGoalBase, IGoalConfig>(config.Goals);
            var injector = _goapConfig.GoapInjector;
            
            goals.ForEach(x =>
            {
                injector.Inject(x);
            });

            return goals;
        }
        
        private List<IWorldSensor> GetWorldSensors(IGoapSetConfig config)
        {
            var worldSensors = _classResolver.Load<IWorldSensor, IWorldSensorConfig>(config.WorldSensors);
            var injector = _goapConfig.GoapInjector;
            
            worldSensors.ForEach(x =>
            {
                injector.Inject(x);
                x.Created();
            });
            
            return worldSensors;
        }
        
        private List<ITargetSensor> GetTargetSensors(IGoapSetConfig config)
        {
            var targetSensors = _classResolver.Load<ITargetSensor, ITargetSensorConfig>(config.TargetSensors);
            var injector = _goapConfig.GoapInjector;
            
            targetSensors.ForEach(x =>
            {
                injector.Inject(x);
                x.Created();
            });
            
            return targetSensors;
        }

        private IAgentDebugger GetDebugger(IGoapSetConfig config)
        {
            return _classResolver.Load<IAgentDebugger>(config.DebuggerClass);
        }
    }
}