using System;
using UnityEngine;

namespace Modules.RunnerGame.Scripts
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