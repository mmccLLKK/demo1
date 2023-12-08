using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

/// <summary>
/// 资源管理器(总)
/// </summary>
public class ResourceManager : Singleton<ResourceManager>, IResourceManager
{
    /// <summary>
    /// 资源存在缓存
    /// </summary>
    protected Dictionary<string, bool> pathContains = new();

    /// <summary>
    /// 
    /// </summary>
    protected Dictionary<string, List<AssetsInfo>> assetsCache = new();

    public UniTask<GameObject> LoadAndInstance(string path)
    {
        throw new System.NotImplementedException();
    }

    public void RemoveInstance(GameObject gameObject, bool isDestroy = true)
    {
    }

    public UniTask<Object> LoadPath(string path)
    {
        throw new System.NotImplementedException();
    }

    public void RemovePath(Object asset)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// 切换场景
    /// </summary>
    public async UniTask<SceneInstance> LoadScene(string path, LoadSceneMode loadMode = LoadSceneMode.Single)
    {
        var sceneInstance = await Addressables.LoadSceneAsync(path, loadMode);
        return sceneInstance;
    }

    public bool CheckResourceExistence(string path)
    {
        if (!pathContains.ContainsKey(path))
        {
            bool result = false;
            // 判断资源是否存在
            foreach (var resourceLocator in Addressables.ResourceLocators)
            {
                if (!resourceLocator.Keys.Contains(path))
                {
                    continue;
                }

                result = true;
            }

            pathContains.Add(path, result);
        }

        return pathContains[path];
    }
}