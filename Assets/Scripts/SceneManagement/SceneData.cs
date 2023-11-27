using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

[CreateAssetMenu(menuName = "ScriptableObjects/Scene Management/Scene Data")]
public class SceneData : ScriptableObject {
    public AssetReference SceneReference;
    public AsyncOperationHandle<SceneInstance> Operation;
}
