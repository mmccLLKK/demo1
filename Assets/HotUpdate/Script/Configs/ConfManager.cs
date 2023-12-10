public class ConfManager : Singleton<ConfManager>
{
    /// <summary>
    /// 关卡配置
    /// </summary>
    public BattleLevelConfigs battleLevelConfigs = new();

    /// <summary>
    /// 角色属性配置
    /// </summary>
    public RoleAttrConfigs attrConfigs = new();

    /// <summary>
    /// 角色模板配置
    /// </summary>
    public RoleTemplateConfigs roleTemplateConfigs = new();

    /// <summary>
    /// 角色能力
    /// </summary>
    public AbilityConfigs abilityConfigs = new();

    /// <summary>
    /// 角色配置
    /// </summary>
    public RoleConfigs roleConfigs = new();
}