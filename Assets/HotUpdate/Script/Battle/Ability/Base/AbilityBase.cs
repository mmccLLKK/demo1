using System;
using UnityEngine;

public abstract class AbilityBase
{
    protected AbilityConfigBase configBase;

    public Role owner;

    public Vector3 dir = Vector3.zero;

    /// <summary>
    /// 实际持续时间
    /// </summary>
    public float timeDuring = 0;

    /// <summary>
    /// 真实时间进度
    /// </summary>
    public float timeProcess = 0;

    /// <summary>
    /// 最大持续时间
    /// </summary>
    public float timeMax = 0;
    
    /// <summary>
    /// 技能timeline
    /// </summary>
    public AbilityTimeline abilityTimeline;

    /// <summary>
    /// 能力状态
    /// </summary>
    public AbilityStatus abilityStatus = 0;

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
    /// 技能tick
    /// </summary>
    /// <param name="deltaTime">变动时间</param>
    public virtual void Tick(float deltaTime)
    {
    }

    /// <summary>
    /// 是否处于释放中
    /// </summary>
    /// <returns></returns>
    public virtual bool IsCasting()
    {
        return this.abilityStatus == AbilityStatus.IS_CASTING;
    }

    /// <summary>
    /// 是否可以退出
    /// </summary>
    public virtual bool CanExit()
    {
        return false;
    }

    /// <summary>
    /// 被打断
    /// </summary>
    public void OnBreak()
    {
    }
}