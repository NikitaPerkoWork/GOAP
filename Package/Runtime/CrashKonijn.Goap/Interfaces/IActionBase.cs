using System;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Resolver.Interfaces;

namespace CrashKonijn.Goap.Interfaces
{
    public interface IActionBase : IAction, IHasConfig<IActionConfig>
    {
        float GetCost(IMonoAgent agent);
        [Obsolete("Please use the IsInRange method")]
        float GetInRange(IMonoAgent agent, IActionData data);
        bool IsInRange(IMonoAgent agent, float distance, IActionData data);
        IActionData GetData();
        void Created();
        public ActionRunState Perform(IMonoAgent agent, IActionData data, ActionContext context);
        void Start(IMonoAgent agent, IActionData data);
        void End(IMonoAgent agent, IActionData data);
    }
}