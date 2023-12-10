using System;
using System.Collections.Generic;
using CrashKonijn.Goap.Configs;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Resolver;

namespace CrashKonijn.Goap.Classes.Builders
{
    public class ActionBuilder
    {
        private readonly ActionConfig _config;
        private readonly List<ICondition> _conditions = new();
        private readonly List<IEffect> _effects = new();
        private readonly WorldKeyBuilder _worldKeyBuilder;
        private readonly TargetKeyBuilder _targetKeyBuilder;

        public ActionBuilder(Type actionType, WorldKeyBuilder worldKeyBuilder, TargetKeyBuilder targetKeyBuilder)
        {
            _worldKeyBuilder = worldKeyBuilder;
            _targetKeyBuilder = targetKeyBuilder;
            
            _config = new ActionConfig
            {
                Name = actionType.Name,
                ClassType = actionType.AssemblyQualifiedName,
                BaseCost = 1,
                InRange = 0.5f
            };
        }

        public ActionBuilder SetTarget<TTargetKey>()
            where TTargetKey : ITargetKey
        {
            _config.Target = _targetKeyBuilder.GetKey<TTargetKey>();
            return this;
        }

        public ActionBuilder SetBaseCost(int baseCost)
        {
            _config.BaseCost = baseCost;
            return this;
        }
        
        public ActionBuilder SetInRange(float inRange)
        {
            _config.InRange = inRange;
            return this;
        }
        
        public ActionBuilder SetMoveMode(ActionMoveMode moveMode)
        {
            _config.MoveMode = moveMode;
            return this;
        }

        public ActionBuilder AddCondition<TWorldKey>(Comparison comparison, int amount)
            where TWorldKey : IWorldKey
        {
            _conditions.Add(new Condition
            {
                WorldKey = _worldKeyBuilder.GetKey<TWorldKey>(),
                Comparison = comparison,
                Amount = amount,
            });
            
            return this;
        }

        [Obsolete("Use `AddEffect<TWorldKey>(EffectType type)` instead.")]
        public ActionBuilder AddEffect<TWorldKey>(bool increase)
            where TWorldKey : IWorldKey
        {
            _effects.Add(new Effect
            {
                WorldKey = _worldKeyBuilder.GetKey<TWorldKey>(),
                Increase = increase
            });
            
            return this;
        }

        public ActionBuilder AddEffect<TWorldKey>(EffectType type)
            where TWorldKey : IWorldKey
        {
            _effects.Add(new Effect
            {
                WorldKey = _worldKeyBuilder.GetKey<TWorldKey>(),
                Increase = type == EffectType.Increase
            });
            
            return this;
        }

        public IActionConfig Build()
        {
            _config.Conditions = _conditions.ToArray();
            _config.Effects = _effects.ToArray();
            return _config;
        }
        
        public static ActionBuilder Create<TAction>(WorldKeyBuilder worldKeyBuilder, TargetKeyBuilder targetKeyBuilder)
            where TAction : IActionBase
        {
            return new ActionBuilder(typeof(TAction), worldKeyBuilder, targetKeyBuilder);
        }
    }
}