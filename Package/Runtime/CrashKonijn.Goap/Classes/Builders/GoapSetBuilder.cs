using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Configs;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Classes.Builders
{
    public class GoapSetBuilder
    {
        private readonly GoapSetConfig _goapSetConfig;

        private readonly List<ActionBuilder> _actionBuilders = new();
        private readonly List<GoalBuilder> _goalBuilders = new();
        private readonly List<TargetSensorBuilder> _targetSensorBuilders = new();
        private readonly List<WorldSensorBuilder> _worldSensorBuilders = new();
        private readonly WorldKeyBuilder _worldKeyBuilder = new();
        private readonly TargetKeyBuilder _targetKeyBuilder = new();

        public GoapSetBuilder(string name)
        {
            _goapSetConfig = new GoapSetConfig(name);
        }

        public ActionBuilder AddAction<TAction>()
            where TAction : IActionBase
        {
            var actionBuilder = ActionBuilder.Create<TAction>(_worldKeyBuilder, _targetKeyBuilder);
            _actionBuilders.Add(actionBuilder);
            return actionBuilder;
        }

        public GoalBuilder AddGoal<TGoal>()
            where TGoal : IGoalBase
        {
            var goalBuilder = GoalBuilder.Create<TGoal>(_worldKeyBuilder);
            _goalBuilders.Add(goalBuilder);
            return goalBuilder;
        }

        public WorldSensorBuilder AddWorldSensor<TWorldSensor>()
            where TWorldSensor : IWorldSensor
        {
            var worldSensorBuilder = WorldSensorBuilder.Create<TWorldSensor>(_worldKeyBuilder);
            _worldSensorBuilders.Add(worldSensorBuilder);
            return worldSensorBuilder;
        }

        public TargetSensorBuilder AddTargetSensor<TTargetSensor>()
            where TTargetSensor : ITargetSensor
        {
            var targetSensorBuilder = TargetSensorBuilder.Create<TTargetSensor>(_targetKeyBuilder);
            _targetSensorBuilders.Add(targetSensorBuilder);
            return targetSensorBuilder;
        }

        public WorldKeyBuilder GetWorldKeyBuilder()
        {
            return _worldKeyBuilder;
        }

        public GoapSetConfig Build()
        {
            _goapSetConfig.Actions = _actionBuilders.Select(x => x.Build()).ToList();
            _goapSetConfig.Goals = _goalBuilders.Select(x => x.Build()).ToList();
            _goapSetConfig.TargetSensors = _targetSensorBuilders.Select(x => x.Build()).ToList();
            _goapSetConfig.WorldSensors = _worldSensorBuilders.Select(x => x.Build()).ToList();

            return _goapSetConfig;
        }

        public GoapSetBuilder SetAgentDebugger<TDebugger>()
            where TDebugger : IAgentDebugger
        {
            _goapSetConfig.DebuggerClass = typeof(TDebugger).AssemblyQualifiedName;

            return this;
        }
    }
}