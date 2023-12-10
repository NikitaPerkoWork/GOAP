using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Behaviours
{
    public class AgentCollection : IAgentCollection
    {
        private readonly HashSet<IMonoAgent> _agents = new();
        private readonly HashSet<IMonoAgent> _queue = new();

        public HashSet<IMonoAgent> All() => _agents;
        
        public void Add(IMonoAgent agent)
        {
            _agents.Add(agent);
        }

        public void Remove(IMonoAgent agent)
        {
            _agents.Remove(agent);
        }

        public void Enqueue(IMonoAgent agent)
        {
            _queue.Add(agent);
        }
        
        public int GetQueueCount() => _queue.Count;

        public IMonoAgent[] GetQueue()
        {
            var data = _queue.ToArray();
            
            _queue.Clear();

            return data;
        }
    }
}