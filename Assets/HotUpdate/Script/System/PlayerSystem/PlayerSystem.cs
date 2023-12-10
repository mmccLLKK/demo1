using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerSystem : Singleton<PlayerSystem>
{
    /// <summary>
    /// 当前玩家数据
    /// </summary>
    public PlayerData curPlayerData;

    /// <summary>
    /// 加载指定玩家数据
    /// </summary>
    /// <returns></returns>
    public async UniTask<PlayerData> LoadPlayerData(string playerId)
    {
        //加载文件

        //反序列化出玩家存档

        //TODO 我们这里先搞一个假的
        PlayerData playerData = new PlayerData
        {
            playerId = "test_player",
            name = "临时玩家",
            createTime = System.DateTime.Now.ToString(),
            //可能有的资源 (金币 物品)  item_id:num
            playerRoleDatas = new List<RoleData>()
            {
                new RoleData
                {
                    roleId = "axceler",
                    lv = 1,
                    abilityIds = new() {"axceler_normal_attack"}
                }
            }
        };
        return playerData;
    }

    /// <summary>
    /// 保存角色数据
    /// </summary>
    public async UniTask SavePlayerData(PlayerData playerData)
    {
    }

    /// <summary>
    /// 设置当前的角色数据
    /// </summary>
    /// <param name="playerData"></param>
    public void SetCurPlayerData(PlayerData playerData)
    {
        this.curPlayerData = playerData;
    }
}