using Cysharp.Threading.Tasks;
using Script.StaticConfig;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class HotUpdateApp : MonoBehaviour
{
    void Start()
    {
        Init().GetAwaiter();
    }

    /// <summary>
    /// 游戏逻辑的初始化
    /// </summary>
    protected async UniTask Init()
    {
        DontDestroyOnLoad(this);

        DontDestroyOnLoad(Camera.main);

        //初始化写死的配置
        StaticConfig.InitConfig();
        
        //加载UI框架
        Object uiRoot = await Addressables.LoadAssetAsync<Object>("Assets/GameMain/Prefabs/UIRoot.prefab");
        var instantiate = Instantiate(uiRoot) as GameObject;
        if (!instantiate)
        {
            return;
        }

        var uiManager = instantiate.GetComponent<UIManager>();
        uiManager.Init();


        //初始化玩家数据
        //TODO 这里暂时还是假流程
        PlayerData playerData = await PlayerSystem.inst.LoadPlayerData("");
        PlayerSystem.inst.SetCurPlayerData(playerData);

        // 打开一个初始UI
        await uiManager.OpenUI("UI_START");
    }
}