using System;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.MainModule.Scripts
{
    [Serializable]
    public class ObjectsPool<T>
    {
        private int capacity;
        private bool isAutoExpand;

        private List<PoolObject<T>> pool;

        public ObjectsPool(int capacity, bool isAutoExpand)
        {
            this.isAutoExpand = isAutoExpand;
            this.capacity = capacity;
            
            pool = new List<PoolObject<T>>(capacity);
        }

        public bool AddObjectToPool(PoolObject<T> newObject)
        {
            if (capacity == pool.Count)
            {
                if (isAutoExpand)
                {
                    capacity++;
                    pool.Capacity = capacity;
                }
                else
                {
                    return false;
                }
            }
            
            pool.Add(newObject);
            return true;
        }

        public PoolObject<T> GetFreeElement()
        {
            foreach (var poolObject in pool)
            {
                if (!poolObject.isActive)
                {
                    poolObject.isActive = true;
                    return poolObject;
                }
            }

            return null;
        }

        public bool IsHasFreeElements()
        {
            foreach (var poolObject in pool)
            {
                if (!poolObject.isActive)
                {
                    return true;
                }
            }

            return false;
        }

        public void ReleaseObject(PoolObject<T> poolObject)
        {
            poolObject.isActive = false;
        }
        
        public void ReleaseFirstOccupiedObject()
        {
            foreach (var poolObject in pool)
            {
                if (poolObject.isActive)
                {
                    ReleaseObject(poolObject);
                    break;
                }
            }
        }
        
        public void OccupyObject(PoolObject<T> poolObject)
        {
            poolObject.isActive = true;
        }
    }
}