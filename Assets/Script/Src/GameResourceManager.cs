using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;

/// <summary>
/// 资源管理器(更新)
/// </summary>
public class GameResourceManager
{
    protected bool autoUpdate = false;

    public GameResourceManager(bool autoUpdate = false)
    {
        this.autoUpdate = autoUpdate;
    }

    /// <summary>
    /// 初始化资源系统
    /// </summary>
    public async UniTask Init()
    {
        await Addressables.InitializeAsync();
        if (autoUpdate)
        {
            // 检查更新
            await _update_address_ables();
        }
    }

    private async UniTask _update_address_ables()
    {
        // 检查文件更新
        // 这一步会根据Addressables中的资源组来依次检查更新
        // 打包后 会 从配置中的RemoteBuildPath中下载资源
        // Addressables 会自动根据catalog中各个资源的hash值来判断是否需要更新
        List<string> catalogs = await Addressables.CheckForCatalogUpdates();

        if (catalogs.Count <= 0)
        {
            //没有需要更新的资源
            Debug.Log("没有需要更新的资源");
            return;
        }

        //需要更新资源  则 根据catalogs 拿到需要更新的资源位置 
        List<IResourceLocator> resourceLocators = await Addressables.UpdateCatalogs(catalogs);
        Debug.Log($"需要更新:{resourceLocators.Count}个资源");

        foreach (IResourceLocator resourceLocator in resourceLocators)
        {
            Debug.Log($"开始下载资源:{resourceLocator}");
            await _download(resourceLocator);
            Debug.Log($"下载资源:{resourceLocator}完成");
        }
    }

    private async UniTask _download(IResourceLocator resourceLocator)
    {
        var size = await Addressables.GetDownloadSizeAsync(resourceLocator.Keys);
        Debug.Log($"更新:{resourceLocator}资源,总大小:{size}");
        if (size <= 0) return;
        var downloadHandle =
            Addressables.DownloadDependenciesAsync(resourceLocator.Keys, Addressables.MergeMode.Union);

        await downloadHandle;

        // Debug.Log("更新完毕!");
        Addressables.Release(downloadHandle);
    }

    /// <summary>
    /// 销毁
    /// </summary>
    public void Destroy()
    {
    }
}