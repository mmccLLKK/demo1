using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// 资源管理器
/// </summary>
public interface IResourceManager
{
    /// <summary>
    /// 加载并实例化
    /// </summary>
    UniTask<GameObject> LoadAndInstance(string path);

    /// <summary>
    /// 移除这个实例
    /// </summary>
    void RemoveInstance(GameObject gameObject, bool isDestroy = true);

    /// <summary>
    /// 加载资源  
    /// </summary>
    UniTask<Object> LoadPath(string path);

    /// <summary>
    /// 移除资源
    /// </summary>
    void RemovePath(Object asset);

    /// <summary>
    /// 判断资源是否存在
    /// </summary>
    bool CheckResourceExistence(string path);
}