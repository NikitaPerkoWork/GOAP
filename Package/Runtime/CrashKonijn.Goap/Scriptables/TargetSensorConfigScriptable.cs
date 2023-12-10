using CrashKonijn.Goap.Attributes;
using CrashKonijn.Goap.Configs.Interfaces;
using UnityEngine;

namespace CrashKonijn.Goap.Scriptables
{
    [CreateAssetMenu(menuName = "Goap/TargetSensorConfig")]
    public class TargetSensorConfigScriptable : ScriptableObject, ITargetSensorConfig
    {
        [TargetSensor]
        public string classType;

        public TargetKeyScriptable key;

        public string Name => name;

        public ITargetKey Key => key;

        public string ClassType
        {
            get => classType;
            set => classType = value;
        }
    }
}