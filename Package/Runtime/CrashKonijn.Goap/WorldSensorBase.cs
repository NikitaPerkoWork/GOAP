using CrashKonijn.Goap.Configs.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace CrashKonijn.Goap
{
    public abstract class WorldSensorBase : MonoBehaviour
    {
        [FormerlySerializedAs("keyScriptable")]
        [SerializeField]
        private IWorldKey _keyScriptable;

        public IWorldKey KeyScriptable => _keyScriptable;
    }
}