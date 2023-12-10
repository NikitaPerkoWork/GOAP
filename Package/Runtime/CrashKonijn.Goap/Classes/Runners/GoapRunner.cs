using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Resolver;
using CrashKonijn.Goap.Resolver.Models;

namespace CrashKonijn.Goap.Classes.Runners
{
    public class GoapRunner : IGoapRunner
    {
        private readonly Dictionary<IGoapSet, GoapSetJobRunner> _goapSets = new();
        private readonly Stopwatch _stopwatch = new();

        public float RunTime { get; private set; }
        public float CompleteTime { get; private set; }

        public void Register(IGoapSet goapSet) => _goapSets.Add(goapSet, new GoapSetJobRunner(goapSet, new GraphResolver(goapSet.GetAllNodes().ToArray(), goapSet.GoapConfig.KeyResolver)));

        public void Run()
        {
            _stopwatch.Restart();
            
            foreach (var runner in _goapSets.Values)
            {
                runner.Run();
            }
            
            RunTime = GetElapsedMs();
                        
            foreach (var agent in Agents)
            {
                if (agent.IsNull())
                {
                    continue;
                }

                agent.Run();
            }
        }

        public void Complete()
        {
            _stopwatch.Restart();
            
            foreach (var runner in _goapSets.Values)
            {
                runner.Complete();
            }
            
            CompleteTime = GetElapsedMs();
        }

        public void Dispose()
        {
            foreach (var runner in _goapSets.Values)
            {
                runner.Dispose();
            }
        }

        private float GetElapsedMs()
        {
            _stopwatch.Stop();
            
            return (float) ((double)_stopwatch.ElapsedTicks / Stopwatch.Frequency * 1000);
        }

        public Graph GetGraph(IGoapSet goapSet) => _goapSets[goapSet].GetGraph();
        public bool Knows(IGoapSet goapSet) => _goapSets.ContainsKey(goapSet);
        public IMonoAgent[] Agents => _goapSets.Keys.SelectMany(x => x.Agents.All()).ToArray();

        public IGoapSet[] GoapSets => _goapSets.Keys.ToArray();

        public IGoapSet GetGoapSet(string id)
        {
            var goapSet = GoapSets.FirstOrDefault(x => x.Id == id);

            if (goapSet == null)
            {
                throw new KeyNotFoundException($"No goapSet with id {id} found");
            }

            return goapSet;
        }

        public int QueueCount => _goapSets.Keys.Sum(x => x.Agents.GetQueueCount());
    }
}