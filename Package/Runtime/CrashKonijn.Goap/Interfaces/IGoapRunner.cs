using CrashKonijn.Goap.Resolver.Models;

namespace CrashKonijn.Goap.Interfaces
{
    public interface IGoapRunner
    {
        void Register(IGoapSet goapSet);
        Graph GetGraph(IGoapSet goapSet);
        bool Knows(IGoapSet goapSet);
        IMonoAgent[] Agents { get; }

        IGoapSet[] GoapSets { get; }

        IGoapSet GetGoapSet(string id);
    }
}