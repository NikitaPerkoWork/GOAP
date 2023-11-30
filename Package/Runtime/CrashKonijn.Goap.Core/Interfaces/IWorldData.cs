﻿using System;
using System.Collections.Generic;
using CrashKonijn.Goap.Core.Enums;

namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface IWorldData
    {
        Dictionary<Type, int> States { get; }
        Dictionary<Type, ITarget> Targets { get; }
        ITarget GetTarget(IAction action);
        void SetState(IWorldKey key, int state);
        void SetState<TKey>(int state) where TKey : IWorldKey;
        void SetTarget(ITargetKey key, ITarget target);
        void SetTarget<TKey>(ITarget target) where TKey : ITargetKey;
        bool IsTrue<TWorldKey>(Comparison comparison, int value);
        bool IsTrue(IWorldKey worldKey, Comparison comparison, int value);
        (bool Exists, int Value) GetWorldValue(Type worldKey);
        ITarget GetTargetValue(Type targetKey);
    }
}