using System;
using System.Linq;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Behaviours
{
    public abstract class GoalBase : IGoalBase
    {
        private IGoalConfig _config;
        public IGoalConfig Config => _config;
        
        public Guid Guid { get; } = Guid.NewGuid();
        public Resolver.Interfaces.IEffect[] Effects { get; } = {};
        public Resolver.Interfaces.ICondition[] Conditions => _config.Conditions.Cast<Resolver.Interfaces.ICondition>().ToArray();

        public void SetConfig(IGoalConfig config)
        {
            _config = config;
        }

        public virtual int GetCost(IWorldData data)
        {
            return _config.BaseCost;
        }
    }
}