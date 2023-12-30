using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角色效果器管理
/// 可以理解为buff的基本单位, 一个复杂的buff就是由一系列复杂的效果组成的
/// </summary>
public class RoleModifier : MonoBehaviour
{
    /// <summary>
    /// 角色身上的效果
    /// </summary>
    protected Dictionary<string, List<ModifierBase>> modifierDic = new();
}