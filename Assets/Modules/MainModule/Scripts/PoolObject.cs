using System;

namespace Modules.MainModule.Scripts
{
    [Serializable]
    public class PoolObject<T>
    {
        public T instance;
        public bool isActive;

        public PoolObject(T instance)
        {
            this.instance = instance;
        }
    }
}