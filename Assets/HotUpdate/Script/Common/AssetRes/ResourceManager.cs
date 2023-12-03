using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

/// <summary>
/// 资源管理器(总)
/// </summary>
public class ResourceManager : IResourceManager
{
    protected Dictionary<string, bool> pathContains = new();

    public UniTask<GameObject> LoadAndInstance(string path)
    {
        throw new System.NotImplementedException();
    }

    public void RemoveInstance(GameObject gameObject)
    {
        throw new System.NotImplementedException();
    }

    public UniTask<Object> LoadPath(string path)
    {
        throw new System.NotImplementedException();
    }

    public void RemovePath(Object asset)
    {
        throw new System.NotImplementedException();
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