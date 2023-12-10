using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Attributes;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Serializables;
using UnityEngine;

namespace CrashKonijn.Goap.Scriptables
{
    [CreateAssetMenu(menuName = "Goap/GoalConfig")]
    public class GoalConfigScriptable : ScriptableObject, IGoalConfig
    {
        [GoalClass]
        public string classType;
        public int baseCost = 1;
        public List<SerializableCondition> conditions;

        public string Name => name;

        public List<ICondition> Conditions => conditions.Cast<ICondition>().ToList();

        public int BaseCost
        {
            get => baseCost;
            set => baseCost = value;
        }

        public string ClassType
        {
            get => classType;
            set => classType = value;
        }
    }
}