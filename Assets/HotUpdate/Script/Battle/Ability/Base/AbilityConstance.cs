/// <summary>
/// 能力阶段枚举
/// </summary>
public enum AbilityStatus
{
    /// <summary>
    /// 是否释放中
    /// </summary>
    IS_CASTING = 1,

    /// <summary>
    /// 是否可以取消
    /// </summary>
    IS_CAN_CANCEL = 1 << 2,

    /// <summary>
    /// 是否蓄力中
    /// </summary>
    IS_ACCUMULATE = 1 << 3,

    /// <summary>
    /// 是否填充中
    /// </summary>
    IS_RECHARING = 1 << 4,
}