public class ConfManager : Singleton<ConfManager>
{
    /// <summary>
    /// 关卡配置
    /// </summary>
    public BattleLevelConfigs battleLevelConfigs;

    /// <summary>
    /// 角色属性配置
    /// </summary>
    public RoleAttrConfigs attrConfigs;

    /// <summary>
    /// 角色模板配置
    /// </summary>
    public RoleTemplateConfigs roleTemplateConfigs;

    /// <summary>
    /// 角色配置
    /// </summary>
    public RoleConfigs roleConfigs;
}