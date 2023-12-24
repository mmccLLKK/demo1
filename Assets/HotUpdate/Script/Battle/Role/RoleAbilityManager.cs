using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoleAbilityManager : MonoBehaviour
{
    /// <summary>
    /// 持有的能力
    /// </summary>
    protected Dictionary<string, AbilityBase> _abilityBases = new();

    /// <summary>
    /// 执行中的能力
    /// </summary>
    protected List<AbilityBase> castingAbility = new();

    private Role _role;

    protected Role role
    {
        get
        {
            if (_role)
            {
                _role = this.gameObject.GetComponent<Role>();
            }

            return _role;
        }
    }

    /// <summary>
    /// 添加技能
    /// </summary>
    /// <param name="abilityId">技能</param>
    /// <param name="configStr">配置</param>
    /// <returns></returns>
    public AbilityBase AddAbility(string abilityId, string configStr)
    {
        //不能重复添加技能
        if (_abilityBases.ContainsKey(abilityId))
        {
            return _abilityBases[abilityId];
        }

        //创建技能
        var abilityBase = AbilityFactory.CreateAbility(abilityId, configStr);
        abilityBase.owner = role;
        _abilityBases.Add(abilityId, abilityBase);
        return abilityBase;
    }

    /// <summary>
    /// 获取一个技能
    /// </summary>
    public AbilityBase GetAbility(string abilityId)
    {
        return _abilityBases.GetValueOrDefault(abilityId, null);
    }

    public List<AbilityBase> GetAllAbilities()
    {
        var abilityBases = _abilityBases.Values.ToList();
        return abilityBases;
    }

    public void RemoveAbility(string abilityId)
    {
        if (_abilityBases.ContainsKey(abilityId))
        {
            _abilityBases.Remove(abilityId);
        }
    }

    /// <summary>
    /// 直接给角色使用一个技能
    /// </summary>
    public void UseAbility(AbilityBase abilityBase)
    {
    }

    /// <summary>
    /// 固定时长的执行器
    /// </summary>
    private void FixedUpdate()
    {
        var fixedDeltaTime = Time.fixedDeltaTime;

        foreach (var abilityBase in castingAbility)
        {
            abilityBase.timeDuring += fixedDeltaTime;
            abilityBase.Tick(fixedDeltaTime);
        }
    }
}