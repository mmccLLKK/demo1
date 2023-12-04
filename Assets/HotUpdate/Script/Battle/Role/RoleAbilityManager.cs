using System.Collections.Generic;

public class RoleAbilityManager
{
    protected List<AbilityBase> _abilityBases = new();

    /// <summary>
    /// 一般都是通过一个配置来初始化一个技能...我们这里先写死.验证逻辑
    /// </summary>
    /// <param name="abilityBase"></param>
    public void AddAbility(AbilityBase abilityBase)
    {
        //避免重复添加
        if (_abilityBases.Contains(abilityBase))
        {
            return;
        }

        _abilityBases.Add(abilityBase);
    }

    public void RemoveAbility(AbilityBase abilityBase)
    {
        _abilityBases.Remove(abilityBase);
    }

    /// <summary>
    /// 直接给角色使用一个技能
    /// </summary>
    public void UseAbility(AbilityBase abilityBase)
    {
    }
}