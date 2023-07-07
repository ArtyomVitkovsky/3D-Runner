using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace Modules.RunnerGame.Scripts
{
    public class AddressablesSystem
    {
        public void InstantiateAsset(AssetReference assetReference, Action<AsyncOperationHandle<GameObject>> action)
        {
            assetReference.InstantiateAsync().Completed += action;
        }
    
        public void ReleaseInstance(AssetReference assetReference, GameObject gameObject)
        {
            assetReference.ReleaseInstance(gameObject);
        }
    
        public void LoadAsset<T>(AssetReferenceT<T> assetReference, Action<AsyncOperationHandle<T>> onComplete) where T : Object
        {
            var asyncOperationHandle = assetReference.LoadAssetAsync<T>();
            asyncOperationHandle.Completed += onComplete;
        }

        public void ReleaseAsset<T>(AssetReferenceT<T> assetReference) where T : Object
        {
            assetReference.ReleaseAsset();
        }
    }
}