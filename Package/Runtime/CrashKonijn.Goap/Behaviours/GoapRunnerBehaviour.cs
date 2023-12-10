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

    public GoapConfigInitializerBase configInitializer;

    [Obsolete(
        "'setConfigFactories' is deprecated, please use 'goapSetConfigFactories' instead.   Exact same functionality, name changed to mitigate confusion with the word 'set' which could have many meanings.")]
    [Header("Obsolete: please use 'GoapSetConfigFactories'")]
    public List<GoapSetFactoryBase> setConfigFactories = new();

    public List<GoapSetFactoryBase> goapSetConfigFactories = new();

    private GoapConfig _config;
    private bool _isInitialized = false;

    private void Awake()
    {
        Initialize();
    }

    public void Register(IGoapSet goapSet) => _runner.Register(goapSet);

    public void Register(IGoapSetConfig goapSetConfig) =>
        _runner.Register(new GoapSetFactory(_config).Create(goapSetConfig));

    private void Update()
    {
        RunCount = _runner.QueueCount;
        _runner.Run();
    }

    private void LateUpdate()
    {
        _runner.Complete();
    }

    private void OnDestroy()
    {
        _runner.Dispose();
    }

    private void Initialize()
    {
        if (_isInitialized)
        {
            return;
        }

        _config = GoapConfig.Default;
        _runner = new Classes.Runners.GoapRunner();

        if (configInitializer != null)
        {
            configInitializer.InitConfig(_config);
        }

        CreateGoapSets();

        _isInitialized = true;
    }

    private void CreateGoapSets()
    {
#pragma warning disable CS0618
        if (setConfigFactories.Any())
        {
            Debug.LogError(
                "setConfigFactory is obsolete. Please move its data to the goapSetConfigFactories using the editor.");
            goapSetConfigFactories.AddRange(setConfigFactories);
        }
#pragma warning restore CS0618

        var goapSetFactory = new GoapSetFactory(_config);

        goapSetConfigFactories.ForEach(factory => { Register(goapSetFactory.Create(factory.Create())); });
    }

    public Graph GetGraph(IGoapSet goapSet) => _runner.GetGraph(goapSet);
    public bool Knows(IGoapSet goapSet) => _runner.Knows(goapSet);
    public IMonoAgent[] Agents => _runner.Agents;

    public IGoapSet[] GoapSets => _runner.GoapSets;

    public IGoapSet GetGoapSet(string id)
    {
        Initialize();

        return _runner.GetGoapSet(id);
    }
}
}