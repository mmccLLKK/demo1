using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class HotUpdateApp : MonoBehaviour
{
    void Start()
    {
        Init();
    }

    /// <summary>
    /// 游戏逻辑的初始化
    /// </summary>
    protected async UniTask Init()
    {
        DontDestroyOnLoad(this);

        //加载UI框架
        Object uiRoot = await Addressables.LoadAssetAsync<Object>("Assets/GameMain/Prefabs/UIRoot.prefab");
        var instantiate = Instantiate(uiRoot) as GameObject;
        if (!instantiate)
        {
            return;
        }

        var uiManager = instantiate.GetComponent<UIManager>();
        uiManager.Init();
        uiManager.OpenUI("UI_START");
    }
}