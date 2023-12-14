using CrashKonijn.Goap.Interfaces;

namespace Demos.Shared
{
    public class AgentDebugger : IAgentDebugger
    {
        public string GetInfo(IMonoAgent agent)
        {
            return $"Hunger:";
        }
    }
}