using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class RoleAbilityManager : MonoBehaviour
{
    protected Dictionary<string, AbilityBase> _abilityBases = new();

    protected Role _role;

    private void Awake()
    {
        _role = GetComponent<Role>();
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
        abilityBase.owner = _role;
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
}