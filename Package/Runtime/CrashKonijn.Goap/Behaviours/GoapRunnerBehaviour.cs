using System;
using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Configs.Interfaces;
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

    public List<GoapSetFactoryBase> goapSetConfigFactories = new();
    
    public void Init(IGoapSet goapSet)
    {
        _runner = new Classes.Runners.GoapRunner(goapSet);
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

    private void OnDestroy()
    {
        _runner.Dispose();
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