using System;
using UnityEngine;

public abstract class AbilityBase
{
    protected AbilityConfigBase configBase;

    public Role owner;

    public Vector3 dir = Vector3.zero;

    /// <summary>
    /// 技能状态
    /// </summary>
    public AbilityStatus abilityStatus = AbilityStatus.CanCast;

    /// <summary>
    /// 委托.当技能释放完成回调
    /// </summary>
    public Action OnAbilityEnd;

    /// <summary>
    /// 初始化
    /// </summary>
    public virtual void Init(AbilityConfigBase configBase)
    {
        this.configBase = configBase;
    }

    public void SetDir(Vector3 dir)
    {
        this.dir = dir;
    }

    public void SetOwner(Role role)
    {
    }

    /// <summary>
    /// 释放开始
    /// </summary>
    public virtual void CasteStart()
    {
    }

    /// <summary>
    /// 被打断
    /// </summary>
    public void OnBreak()
    {
    }
}