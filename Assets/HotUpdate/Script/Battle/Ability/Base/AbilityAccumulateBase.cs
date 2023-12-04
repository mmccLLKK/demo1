using System;

/// <summary>
/// 蓄力技能基础类
/// </summary>
public class AbilityAccumulateBase : AbilityBase
{
    /// <summary>
    /// 委托.当技能蓄力满
    /// </summary>
    public Action OnAccumulateMax;
}