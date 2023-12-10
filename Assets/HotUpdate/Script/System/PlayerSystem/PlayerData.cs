using System.Collections.Generic;

/// <summary>
/// 玩家数据(可以说就是存档)
/// </summary>
public class PlayerData
{
    /// <summary>
    /// 玩家id
    /// </summary>
    public string playerId;

    /// <summary>
    /// 玩家昵称
    /// </summary>
    public string name;

    /// <summary>
    /// 存档创建时间
    /// </summary>
    public string createTime;

    /// <summary>
    /// 角色拥有的角色
    /// </summary>
    public List<RoleData> playerRoleDatas;
}