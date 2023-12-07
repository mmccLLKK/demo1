using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

/// <summary>
/// 关卡管理器
/// 游戏设计为关卡模式.箱庭式的战斗方式
/// 这里主要是管理进入关卡的所有流程,以及战斗数据的填充
/// </summary>
public class LevelManager
{
    /// <summary>
    /// 切换到对应的关卡
    /// </summary>
    /// <param name="levelId"></param>
    /// <returns></returns>
    public static async UniTask LoadLevel(string levelId)
    {
        //初始化关卡信息
        var battleLevelConfig = ConfManager.inst.battleLevelConfigs.GetLevelById(levelId);
        if (battleLevelConfig == null)
        {
            Debug.Log($"关卡信息不存在 {levelId}");
            return;
        }

        //切换场景
        var scene = await Addressables.LoadSceneAsync(battleLevelConfig.path);
        //初始化场景信息
        var rootGameObjects = scene.Scene.GetRootGameObjects();
        if (rootGameObjects.Length != 1)
        {
            Debug.Log($"场景 {scene.Scene.name} 根路径不符合规范.应该只有一个root节点");
            return;
        }

        var rootGameObject = rootGameObjects[0];
        var battleManager = rootGameObject.AddComponent<BattleManager>();
        //等待一帧保证添加的物体走完自己的生命周期
        await Task.Delay(1);
        //初始化角色信息
        
        //
    }
}