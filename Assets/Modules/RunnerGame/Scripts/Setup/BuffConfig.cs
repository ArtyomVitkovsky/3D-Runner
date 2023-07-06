using System;
using UnityEngine;

namespace Modules.RunnerGame.Scripts.Setup
{
    [Serializable]
    public class BuffConfig
    {
        [SerializeField] protected GameObject prefab;
        [SerializeField] protected int chance;

        public int Chance => chance;

        public GameObject Prefab => prefab;
    }
}