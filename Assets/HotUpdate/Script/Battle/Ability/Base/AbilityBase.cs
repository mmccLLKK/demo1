using System;
using UnityEngine;

public abstract class AbilityBase
{
    public Role owner;
    public Vector3 dir = Vector3.zero;

    /// <summary>
    /// 技能状态
    /// </summary>
    public AbilityStatus abilityStatus;

    /// <summary>
    /// 委托.当技能释放完成回调
    /// </summary>
    public Action OnAbilityEnd;

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
    public void CasteStart()
    {
    }

    /// <summary>
    /// 被打断
    /// </summary>
    public void OnBreak()
    {
    }
}