public class RoleConfig
{
    public string id;
    public string roleAttrId;
    public string roleTemplateId;
}

public class RoleConfigs : ConfigsBase<RoleConfig>
{
    /// <summary>
    /// 获取角色配置
    /// </summary>
    public RoleConfig getRoleConfigById(string roleId)
    {
        var roleConfig = this.tables.Find(table => table.id.Equals(roleId));
        return roleConfig;
    }
}