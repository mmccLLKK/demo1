public class RoleTemplateConfig
{
    public string id;
    public string name;
    public string path;
}

public class RoleTemplateConfigs : ConfigsBase<RoleTemplateConfig>
{
    /// <summary>
    /// 获取角色模板配置
    /// </summary>
    public RoleTemplateConfig getRoleTemplateConfigById(string roleId)
    {
        var roleTemplateConfig = this.tables.Find(table => table.id.Equals(roleId));
        return roleTemplateConfig;
    }
}