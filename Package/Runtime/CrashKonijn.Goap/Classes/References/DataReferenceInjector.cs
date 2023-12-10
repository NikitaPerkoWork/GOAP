using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;

namespace CrashKonijn.Goap.Classes.References
{
    public class DataReferenceInjector : IDataReferenceInjector
    {
        private readonly IMonoAgent _agent;
        private readonly Dictionary<Type, object> _references = new();

        public DataReferenceInjector(IMonoAgent agent)
        {
            _agent = agent;
        }

        public void Inject(IActionData data)
        {
            var type = data.GetType();

            // find all properties with the GetComponent attribute
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                var value = GetPropertyValue(property);

                if (value == null)
                {
                    continue;
                }

                // set the reference
                property.SetValue(data, value);
            }
        }

        private object GetPropertyValue(PropertyInfo property)
        {
            if (property.GetCustomAttributes(typeof(GetComponentAttribute), true).Any())
            {
                return GetCachedComponentReference(property.PropertyType);
            }

            if (property.GetCustomAttributes(typeof(GetComponentInChildrenAttribute), true).Any())
            {
                return GetCachedComponentInChildrenReference(property.PropertyType);
            }

            if (property.GetCustomAttributes(typeof(GetComponentInParentAttribute), true).Any())
            {
                return GetCachedComponentInParentReference(property.PropertyType);
            }

            return null;
        }

        private object GetCachedComponentReference(Type type)
        {
            // check if we have a reference for this type
            if (!_references.ContainsKey(type))
            {
                _references.Add(type, _agent.GetComponent(type));
            }

            // get the reference
            return _references[type];
        }

        public T GetCachedComponent<T>()
            where T : MonoBehaviour
        {
            return (T)GetCachedComponentReference(typeof(T));
        }

        private object GetCachedComponentInChildrenReference(Type type)
        {
            // check if we have a reference for this type
            if (!_references.ContainsKey(type))
            {
                _references.Add(type, _agent.GetComponentInChildren(type));
            }

            // get the reference
            return _references[type];
        }

        public T GetCachedComponentInChildren<T>()
            where T : MonoBehaviour
        {
            return (T)GetCachedComponentInChildrenReference(typeof(T));
        }

        private object GetCachedComponentInParentReference(Type type)
        {
            // check if we have a reference for this type
            if (!_references.ContainsKey(type))
            {
                _references.Add(type, _agent.GetComponentInParent(type));
            }

            // get the reference
            return _references[type];
        }

        public T GetCachedComponentInParent<T>() where T : MonoBehaviour
        {
            return (T)GetCachedComponentInParentReference(typeof(T));
        }
    }
}