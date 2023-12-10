using System;
using System.Collections.Generic;

namespace CrashKonijn.Goap.Classes.Builders
{
    public abstract class KeyBuilderBase<TInterface>
    {
        private readonly Dictionary<Type, TInterface> _keys = new();

        public TInterface GetKey<TKey>()
            where TKey : TInterface
        {
            var type = typeof(TKey);
            
            if (_keys.TryGetValue(type, out var key))
            {
                return key;
            }

            key = (TInterface) Activator.CreateInstance(type);
            
            InjectData(key);
            _keys.Add(type, key);
            
            return key;
        }

        protected abstract void InjectData(TInterface key);
    }
}