using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Resolver;
using CrashKonijn.Goap.Resolver.Interfaces;
using CrashKonijn.Goap.Resolver.Models;
using Unity.Collections;
using Unity.Mathematics;

namespace CrashKonijn.Goap.Classes.Runners
{
    public class GoapSetJobRunner
    {
        private readonly IGoapSet _goapSet;
        private readonly IGraphResolver _resolver;
        private readonly List<JobRunHandle> _resolveHandles = new();
        private readonly IExecutableBuilder _executableBuilder;
        private readonly IPositionBuilder _positionBuilder;
        private readonly ICostBuilder _costBuilder;
        private readonly IConditionBuilder _conditionBuilder;

        public GoapSetJobRunner(IGoapSet goapSet, IGraphResolver graphResolver)
        {
            _goapSet = goapSet;
            _resolver = graphResolver;
            _executableBuilder = _resolver.GetExecutableBuilder();
            _positionBuilder = _resolver.GetPositionBuilder();
            _costBuilder = _resolver.GetCostBuilder();
            _conditionBuilder = _resolver.GetConditionBuilder();
        }

        public void Run()
        {
            _resolveHandles.Clear();
            _goapSet.SensorRunner.Update();
            var globalData = _goapSet.SensorRunner.SenseGlobal();
            foreach (var agent in _goapSet.Agents.GetQueue())
            {
                Run(globalData, agent);
            }
        }

        private void Run(GlobalWorldData globalData, IMonoAgent agent)
        {
            if (agent.IsNull())
            {
                return;
            }

            if (agent.CurrentGoal == null)
            {
                return;
            }

            var localData = _goapSet.SensorRunner.SenseLocal(globalData, agent);

            if (IsGoalCompleted(localData, agent))
            {
                var goal = agent.CurrentGoal;
                agent.ClearGoal();
                agent.Events.GoalCompleted(goal);
                return;
            }

            FillBuilders(localData, agent);
            
            _resolveHandles.Add(new JobRunHandle(agent)
            {
                Handle = _resolver.StartResolve(new RunData
                {
                    StartIndex = _resolver.GetIndex(agent.CurrentGoal),
                    IsExecutable = new NativeArray<bool>(_executableBuilder.Build(), Allocator.TempJob),
                    Positions = new NativeArray<float3>(_positionBuilder.Build(), Allocator.TempJob),
                    Costs = new NativeArray<float>(_costBuilder.Build(), Allocator.TempJob),
                    ConditionsMet = new NativeArray<bool>(_conditionBuilder.Build(), Allocator.TempJob),
                    DistanceMultiplier = agent.DistanceMultiplier
                })
            });
        }

        private void FillBuilders(LocalWorldData localData, IMonoAgent agent)
        {
            var conditionObserver = _goapSet.GoapConfig.ConditionObserver;
            conditionObserver.SetWorldData(localData);

            _executableBuilder.Clear();
            _positionBuilder.Clear();
            _conditionBuilder.Clear();

            var transformTarget = new TransformTarget(agent.transform);

            foreach (var node in _goapSet.GetActions())
            {
                var allMet = true;
                
                foreach (var condition in node.Conditions)
                {
                    if (!conditionObserver.IsMet(condition))
                    {
                        allMet = false;
                        continue;
                    }

                    _conditionBuilder.SetConditionMet(condition, true);
                }
                
                var target = localData.GetTarget(node);

                _executableBuilder.SetExecutable(node, allMet);
                _costBuilder.SetCost(node, node.GetCost(agent, agent.Injector));
                
                _positionBuilder.SetPosition(node, target?.Position ?? transformTarget.Position);
            }
        }

        private bool IsGoalCompleted(LocalWorldData localData, IMonoAgent agent)
        {
            var conditionObserver = _goapSet.GoapConfig.ConditionObserver;
            conditionObserver.SetWorldData(localData);
            
            return agent.CurrentGoal.Conditions.All(x => conditionObserver.IsMet(x));
        }

        public void Complete()
        {
            foreach (var resolveHandle in _resolveHandles)
            {
                var result = resolveHandle.Handle.Complete().OfType<IActionBase>().ToList();

                if (resolveHandle.Agent.IsNull())
                {
                    continue;
                }

                var action = result.FirstOrDefault();
                if (action is null)
                {
                    resolveHandle.Agent.Events.NoActionFound(resolveHandle.Agent.CurrentGoal);
                    continue;
                }

                if (action != resolveHandle.Agent.CurrentAction)
                {
                    resolveHandle.Agent.SetAction(action, result, resolveHandle.Agent.WorldData.GetTarget(action));
                }
            }

            _resolveHandles.Clear();
        }

        public void Dispose()
        {
            foreach (var resolveHandle in _resolveHandles)
            {
                resolveHandle.Handle.Complete();
            }
            
            _resolver.Dispose();
        }

        private class JobRunHandle
        {
            public IMonoAgent Agent { get; }
            public IResolveHandle Handle { get; set; }
            
            public JobRunHandle(IMonoAgent agent)
            {
                Agent = agent;
            }
        }

        public Graph GetGraph() => _resolver.GetGraph();
    }
}