using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Resolver.Interfaces;

namespace CrashKonijn.Goap.Classes
{
    public class GoapSet : IGoapSet
    {
        public string Id { get; }
        public IAgentCollection Agents { get; } = new AgentCollection();
        public IGoapConfig GoapConfig { get; }
        public ISensorRunner SensorRunner { get; }
        public IAgentDebugger Debugger { get; }

        private readonly List<IGoalBase> _goals;
        private readonly List<IActionBase> _actions;

        public GoapSet(string id, IGoapConfig config, List<IGoalBase> goals, List<IActionBase> actions, ISensorRunner sensorRunner, IAgentDebugger debugger)
        {
            Id = id;
            GoapConfig = config;
            SensorRunner = sensorRunner;
            _goals = goals;
            _actions = actions;
            Debugger = debugger;
        }

        public void Register(IMonoAgent agent) => Agents.Add(agent);
        public void Unregister(IMonoAgent agent) => Agents.Remove(agent);

        public TAction ResolveAction<TAction>()
            where TAction : ActionBase
        {
            var result = _actions.FirstOrDefault(x => x.GetType() == typeof(TAction));

            if (result != null)
            {
                return (TAction) result;
            }

            throw new KeyNotFoundException($"No action found of type {typeof(TAction)}");
        }

        public TGoal ResolveGoal<TGoal>()
            where TGoal : IGoalBase
        {
            var result = _goals.FirstOrDefault(x => x.GetType() == typeof(TGoal));

            if (result != null)
            {
                return (TGoal) result;
            }

            throw new KeyNotFoundException($"No action found of type {typeof(TGoal)}");
        }

        public List<IAction> GetAllNodes() => _actions.Cast<IAction>().Concat(_goals).ToList();
        public List<IActionBase> GetActions() => _actions;
        public List<IGoalBase> GetGoals() => _goals;

        public AgentDebugGraph GetDebugGraph()
        {
            return new AgentDebugGraph
            {
                Goals = _goals,
                Actions = _actions,
                Config = GoapConfig
            };
        }
    }
}