using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

/// <summary>
/// 关卡管理器
/// 游戏设计为关卡模式.箱庭式的战斗方式
/// 这里主要是管理进入关卡的所有流程,以及战斗数据的填充
/// </summary>
public class BattleLevelManager
{
    /// <summary>
    /// 切换到对应的关卡
    /// </summary>
    /// <param name="levelId"></param>
    /// <returns></returns>
    public static async UniTask<BattleWorld> LoadLevel(string levelId, PlayerData playerData)
    {
        //初始化关卡信息
        var battleLevelConfig = ConfManager.inst.battleLevelConfigs.GetLevelById(levelId);
        if (battleLevelConfig == null)
        {
            Debug.Log($"关卡信息不存在 {levelId}");
            return null;
        }

        //切换场景
        var scene = await Addressables.LoadSceneAsync(battleLevelConfig.path);
        //初始化场景信息
        var rootGameObjects = scene.Scene.GetRootGameObjects();
        if (rootGameObjects.Length != 1)
        {
            Debug.Log($"场景 {scene.Scene.name} 根路径不符合规范.应该只有一个root节点");
            return null;
        }

        var rootGameObject = rootGameObjects[0];
        var battleWorld = rootGameObject.AddComponent<BattleWorld>();
        //等待一帧保证添加的物体走完自己的生命周期
        await Task.Delay(1);
        //需要有关起的配置.来进行初始化加载
        await battleWorld.Init();
        await battleWorld.InitUI();
        //场景和各种机制加载完成过后 开始 初始化角色信息. 并且把角色投入战场
        foreach (var roleData in playerData.playerRoleDatas)
        {
            var (instId, role) = await battleWorld.CreateRole(roleData);
            battleWorld.PlayerBindRole(playerData.playerId, instId);
            //这里简单处理一下出生点
            role.transform.position = battleWorld.roleBornPoint.transform.position;
        }


        return battleWorld;
    }
}