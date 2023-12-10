using CrashKonijn.Goap.Interfaces;
using UnityEngine;

namespace CrashKonijn.Goap.Classes
{
    public class TransformTarget : ITarget
    {
        public Transform Transform { get; }

        public Vector3 Position
        {
            get
            {
               if (Transform == null)
               {
                   return Vector3.zero;
               }

               return Transform.position;
            }
        }

        public TransformTarget(Transform transform)
        {
            Transform = transform;
        }
    }
}