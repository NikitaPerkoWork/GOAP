using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Attributes;
using CrashKonijn.Goap.Configs.Interfaces;
using UnityEngine;

namespace CrashKonijn.Goap.Scriptables
{
    [CreateAssetMenu(menuName = "Goap/GoapSetConfig")]
    public class GoapSetConfigScriptable : ScriptableObject, IGoapSetConfig
    {
        public List<ActionConfigScriptable> actions = new List<ActionConfigScriptable>();
        public List<GoalConfigScriptable> goals = new List<GoalConfigScriptable>();

        public List<TargetSensorConfigScriptable> targetSensors = new List<TargetSensorConfigScriptable>();
        public List<WorldSensorConfigScriptable> worldSensors = new List<WorldSensorConfigScriptable>();

        [ActionDebuggerClass]
        public string debuggerClass;

        public string Name => name;
        public List<IActionConfig> Actions => actions.Cast<IActionConfig>().ToList();
        public List<IGoalConfig> Goals => goals.Cast<IGoalConfig>().ToList();
        public List<ITargetSensorConfig> TargetSensors => targetSensors.Cast<ITargetSensorConfig>().ToList();
        public List<IWorldSensorConfig> WorldSensors => worldSensors.Cast<IWorldSensorConfig>().ToList();
        public string DebuggerClass => debuggerClass;
    }
}