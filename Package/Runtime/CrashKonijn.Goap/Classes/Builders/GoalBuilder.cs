using System;
using System.Collections.Generic;
using CrashKonijn.Goap.Configs;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Resolver;

namespace CrashKonijn.Goap.Classes.Builders
{
    public class GoalBuilder
    {
        private readonly GoalConfig _config;
        private readonly List<ICondition> _conditions = new();
        private readonly WorldKeyBuilder _worldKeyBuilder;

        public GoalBuilder(Type type, WorldKeyBuilder worldKeyBuilder)
        {
            _worldKeyBuilder = worldKeyBuilder;
            _config = new GoalConfig(type)
            {
                BaseCost = 1,
                ClassType = type.AssemblyQualifiedName
            };
        }
        
        public GoalBuilder SetBaseCost(int baseCost)
        {
            _config.BaseCost = baseCost;
            return this;
        }
        
        public GoalBuilder AddCondition<TWorldKey>(Comparison comparison, int amount)
            where TWorldKey : IWorldKey
        {
            _conditions.Add(new Condition(_worldKeyBuilder.GetKey<TWorldKey>(), comparison, amount));
            return this;
        }
        
        public IGoalConfig Build()
        {
            _config.Conditions = _conditions;
            return _config;
        }
        
        public static GoalBuilder Create<TGoal>(WorldKeyBuilder worldKeyBuilder)
            where TGoal : IGoalBase
        {
            return new GoalBuilder(typeof(TGoal), worldKeyBuilder);
        }
    }
}