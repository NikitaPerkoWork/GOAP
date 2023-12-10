using System;
using System.Linq;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using ICondition = CrashKonijn.Goap.Resolver.Interfaces.ICondition;
using IEffect = CrashKonijn.Goap.Resolver.Interfaces.IEffect;

namespace CrashKonijn.Goap.Behaviours
{
    public abstract class ActionBase<TActionData> : ActionBase
        where TActionData : IActionData, new()
    {
        public override IActionData GetData()
        {
            return CreateData();
        }

        public virtual TActionData CreateData()
        {
            return new TActionData();
        }

        public override void Start(IMonoAgent agent, IActionData data) => Start(agent, (TActionData) data);
        
        public abstract void Start(IMonoAgent agent, TActionData data);

        public override ActionRunState Perform(IMonoAgent agent, IActionData data, ActionContext context) => Perform(agent, (TActionData) data, context);

        public abstract ActionRunState Perform(IMonoAgent agent, TActionData data, ActionContext context);

        public override void End(IMonoAgent agent, IActionData data) => End(agent, (TActionData) data);
        
        public abstract void End(IMonoAgent agent, TActionData data);
    }

    public abstract class ActionBase : IActionBase
    {
        private IActionConfig _config;
        
        public IActionConfig Config => _config;
        
        public Guid Guid { get; } = Guid.NewGuid();
        public IEffect[] Effects => _config.Effects.Cast<IEffect>().ToArray();
        public ICondition[] Conditions => _config.Conditions.Cast<ICondition>().ToArray();

        public void SetConfig(IActionConfig config)
        {
            _config = config;
        }

        public virtual float GetCost(IMonoAgent agent, IComponentReference references)
        {
            return _config.BaseCost;
        }
        
        [Obsolete("Please use the IsInRange method")]
        public virtual float GetInRange(IMonoAgent agent, IActionData data)
        {
            return _config.InRange;
        }
        
        public virtual bool IsInRange(IMonoAgent agent, float distance, IActionData data, IComponentReference references)
        {
#pragma warning disable CS0618
            return distance <= GetInRange(agent, data);
#pragma warning restore CS0618
        }

        public abstract IActionData GetData();
        public abstract void Created();
        public abstract ActionRunState Perform(IMonoAgent agent, IActionData data, ActionContext context);
        public abstract void Start(IMonoAgent agent, IActionData data);
        public abstract void End(IMonoAgent agent, IActionData data);
    }
}