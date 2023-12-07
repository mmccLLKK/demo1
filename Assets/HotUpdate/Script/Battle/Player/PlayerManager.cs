using System.Collections.Generic;

/// <summary>
/// 玩家管理器
/// </summary>
public class PlayerManager
{
    /// <summary>
    /// 所有玩家
    /// </summary>
    protected Dictionary<int, Player> players = new();

    /// <summary>
    /// 
    /// </summary>
    protected Dictionary<int, PlayerData> playerDatas = new();
}