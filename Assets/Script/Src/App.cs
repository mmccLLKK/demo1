using System;
using System.Reflection;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using HybridCLR;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

public class App : MonoBehaviour
{
    protected GameResourceManager gameResourceManager;

    protected HybridCLRLoader hybridClrLoader;

    public GameObject flashCanvas;

    /// <summary>
    /// 是否自动更新资源
    /// </summary>
    public bool autoUpdateRes = false;

    private void Start()
    {
        Application.targetFrameRate = 60;
        DontDestroyOnLoad(this);
        Init();
    }

    protected async UniTask Init()
    {
        flashCanvas.gameObject.SetActive(true);

        // 初始化资源框架
        gameResourceManager = new GameResourceManager(autoUpdateRes);
        await gameResourceManager.Init();
        // 加载dll热更
        hybridClrLoader = new HybridCLRLoader();
        await hybridClrLoader.Loader();
        // 后续流程再热更代码中
        await hybridClrLoader.Run();

        // 让你看看闪屏
        await Task.Delay(4000);
        flashCanvas.gameObject.SetActive(false);
    }
}