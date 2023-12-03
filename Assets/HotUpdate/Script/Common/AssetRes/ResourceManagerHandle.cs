using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

public struct HandleOpt
{
    public Func<string, bool> checkExist;
}


/// <summary>
/// 资源管理器句柄(分)
/// </summary>
public class ResourceManagerHandle : IResourceManager
{
    protected Func<string, bool> checkExist;

    public ResourceManagerHandle(HandleOpt opt)
    {
        this.checkExist = opt.checkExist;
    }

    public UniTask<GameObject> LoadAndInstance(string path)
    {
        throw new NotImplementedException();
    }

    public void RemoveInstance(GameObject gameObject)
    {
        throw new NotImplementedException();
    }

    public UniTask<Object> LoadPath(string path)
    {
        throw new NotImplementedException();
    }

    public void RemovePath(Object asset)
    {
        throw new NotImplementedException();
    }

    public bool CheckResourceExistence(string path)
    {
        if (this.checkExist == null)
        {
            throw new Exception("资源检查函数不存在");
        }

        return this.checkExist.Invoke(path);
    }
}