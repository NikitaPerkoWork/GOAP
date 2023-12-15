using System.Collections.Generic;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Resolver.Models;
using UnityEngine;

namespace CrashKonijn.Goap.Behaviours
{
[DefaultExecutionOrder(-100)]
public class GoapRunnerBehaviour : MonoBehaviour, IGoapRunner
{
    private Classes.Runners.GoapRunner _runner;

    public float RunTime => _runner.RunTime;
    public float CompleteTime => _runner.CompleteTime;
    public int RunCount { get; private set; }
    
    public void Init(IGoapSet goapSet)
    {
        _runner = new Classes.Runners.GoapRunner(goapSet);
    }

    public void Deinit()
    {
        _runner.Dispose();
    }

    public void Tick()
    {
        RunCount = _runner.QueueCount;
        _runner.Run();
    }

    public void LateTick()
    {
        _runner.Complete();
    }

    public Graph GetGraph(IGoapSet goapSet) => _runner.GetGraph(goapSet);
    public bool Knows(IGoapSet goapSet) => _runner.Knows(goapSet);
    public IMonoAgent[] Agents => _runner.Agents;

    public IGoapSet[] GoapSets => _runner.GoapSets;

    public IGoapSet GetGoapSet(string id)
    {
        return _runner.GetGoapSet(id);
    }
}
}