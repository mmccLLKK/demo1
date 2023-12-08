public class RoleAttrConfig
{
    public string id;
    public int level;
    public int atk;
    public int hp;
}

public class RoleAttrConfigs : ConfigsBase<RoleAttrConfig>
{
    /// <summary>
    /// 通过属性和等级确定属性
    /// </summary>
    public RoleAttrConfig GetConfigByIdAndLv(string id, int level)
    {
        var roleAttrConfig = this.tables.Find(table => table.id.Equals(id) && table.level == level);
        return roleAttrConfig;
    }
}