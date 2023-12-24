namespace Script.Battle.Ability.Axceler
{
    /// <summary>
    /// 当前技能配置
    /// </summary>
    [AbilityConfig("axceler_normal_attack")]
    public class Ability_Normal_Attack_Config : AbilityConfigBase
    {
    }

    /// <summary>
    /// 暂时没有技能编辑器时候的特殊实现
    /// 这里先直接写死
    /// </summary>
    [AbilityType("axceler_normal_attack")]
    public class Ability_Normal_Attack : AbilityMultipleCastBase
    {
    }
}