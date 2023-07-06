using System;
using UnityEngine;

namespace Modules.RunnerGame.Scripts.Setup
{
    [Serializable]
    public struct PlatformConfig
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private PlatformType type;
        [SerializeField] private float spawnChance;

        public GameObject Prefab => prefab;

        public PlatformType Type => type;

        public float SpawnChance => spawnChance;
    }
}