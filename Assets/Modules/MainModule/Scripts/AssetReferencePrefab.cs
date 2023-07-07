using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Modules.MainModule.Scripts
{
    [Serializable]
    public class AssetReferencePrefab : AssetReferenceT<GameObject>
    {
        public AssetReferencePrefab(string guid) : base(guid)
        {
        }
    }
}