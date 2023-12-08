using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// 缺乏脚本语言的灵活,只能这样
/// </summary>
public struct HandleOpt
{
    public Func<string, bool> checkExist;
}


/// <summary>
/// 资源管理器句柄(分)
/// 后续再实现.现在先不管
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

    public void RemoveInstance(GameObject gameObject, bool isDestroy = true)
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