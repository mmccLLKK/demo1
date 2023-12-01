using System;
using System.Reflection;
using Cysharp.Threading.Tasks;
using HybridCLR;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

/// <summary>
/// 加载dll热更
/// </summary>
public class HybridCLRLoader
{
    public async UniTask Loader()
    {
#if UNITY_EDITOR
        // HybridCLR 在 Editor 下没办法使用代码模式
        Debug.Log("Editor模式下.不适用dll热更新");
#else
        //加载热更代码
        await LoadBytes($"Assets/GameMain/Dlls", tuple =>
        {
            var (name, bytes) = tuple;
            Assembly.Load(bytes);
        });
        //加载AOT元数据
        await LoadBytes($"Assets/GameMain/AOTDlls", tuple =>
        {
            var (name, bytes) = tuple;
            // 加载assembly对应的dll，会自动为它hook。一旦aot泛型函数的native函数不存在，用解释器版本代码
            LoadImageErrorCode err = RuntimeApi.LoadMetadataForAOTAssembly(bytes, HomologousImageMode.Consistent);
            if (err == LoadImageErrorCode.OK)
                Debug.Log($"LoadMetadataForAOTAssembly:{name}. ret:{err}");
            else
                Debug.LogError($"LoadMetadataForAOTAssembly:{name}. ret:{err}");
        });
#endif
    }

    /// <summary>
    /// 跑起来
    /// </summary>
    public async UniTask Run()
    {
        //直接加载让代码跑起来
        Object asset = await Addressables.LoadAssetAsync<Object>("Assets/GameMain/Prefabs/HotUpdateApp.prefab");
        GameObject.Instantiate(asset);
    }

    protected async UniTask LoadBytes(string infoPath, Action<(string, byte[])> action)
    {
        var dllInfo = await Addressables.LoadAssetAsync<TextAsset>($"{infoPath}/info.txt");
        var dlls = JsonConvert.DeserializeObject<string[]>(dllInfo.text);
        if (dlls == null)
        {
            return;
        }

        foreach (var dll in dlls)
        {
            var dllAssets = await Addressables.LoadAssetAsync<TextAsset>($"{infoPath}/{dll}.bytes");
            if (!dllAssets)
            {
                continue;
            }

            var dllAssetsName = dllAssets.name;
            // Debug.Log("加载到DLL:" + dllAssetsName);
            action?.Invoke((dllAssetsName, dllAssets.bytes));
        }
    }
}