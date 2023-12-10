using System.Collections.Generic;

/// <summary>
/// 单个角色数据
/// </summary>
public class RoleData
{
    /// <summary>
    /// 角色
    /// </summary>
    public string roleId;

    /// <summary>
    /// 等级
    /// </summary>
    public int lv;

    /// <summary>
    /// 能力(这里是指技能)
    /// </summary>
    public List<string> abilitys;
}