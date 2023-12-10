using UnityEngine;

namespace CrashKonijn.Goap.Interfaces
{
    public interface IComponentReference
    {
        T GetCachedComponent<T>()
            where T : MonoBehaviour;

        T GetCachedComponentInChildren<T>()
            where T : MonoBehaviour;

        T GetCachedComponentInParent<T>()
            where T : MonoBehaviour;
    }
}