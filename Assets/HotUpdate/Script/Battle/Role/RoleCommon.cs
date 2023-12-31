using UnityEngine;

/// <summary>
/// 角色状态控制
/// </summary>
public struct RoleCtrlStatus
{
    /// <summary>
    /// 是否可移动（一般指主动）
    /// </summary>
    public bool canMove;

    /// <summary>
    /// 是否可旋转
    /// </summary>
    public bool canRotate;

    /// <summary>
    /// 是否能够攻击（释放技能）
    /// </summary>
    public bool canAttack;

    /// <summary>
    /// 自由状态
    /// </summary>
    public static RoleCtrlStatus free = new()
    {
        canMove = true,
        canRotate = true,
        canAttack = true,
    };

    /// <summary>
    /// 目前状态只能叠加
    /// </summary>
    public static RoleCtrlStatus operator +(RoleCtrlStatus left, RoleCtrlStatus right)
    {
        left.canMove &= right.canMove;
        left.canRotate &= right.canRotate;
        left.canAttack &= right.canAttack;
        return left;
    }
}

/// <summary>
/// 角色属性(叠加)
/// 一般的数值游戏，会使用定义好这些属性（因为数值计算量特别大）
/// 某些游戏喜欢构建属性依赖树来处理属性
/// ================================ 实际值的计算依然可以使用公式完成
/// </summary>
public struct RoleAttrValue
{
    /// <summary>
    /// 生命值
    /// </summary>
    public float hp;

    /// <summary>
    /// 移动速度
    /// </summary>
    public float moveSpeed;

    /// <summary>
    /// 攻击力
    /// </summary>
    public float atk;

    /// <summary>
    /// 防御力
    /// </summary>
    public float def;

    /// <summary>
    /// 暴击率
    /// </summary>
    public float crit;

    public static RoleAttrValue origin = new()
    {
        hp = 0f,
        moveSpeed = 0f,
        atk = 0f,
        def = 0f,
        crit = 0f,
    };

    public static RoleAttrValue operator +(RoleAttrValue left, RoleAttrValue right)
    {
        //这里就不做血量最大值处理了,所见即所得
        left.hp += right.hp;
        left.moveSpeed += right.moveSpeed;
        left.atk += right.atk;
        left.def += right.def;
        left.crit += right.crit;
        return left;
    }
}


/// <summary>
/// 动画信息
/// </summary>
public class AnimInfo
{
    /// <summary>
    /// id 移除的时候需要使用的
    /// </summary>
    public int id;

    /// <summary>
    /// 动画名
    /// </summary>
    public string animName;

    /// <summary>
    /// 优先级 (需要配置,暂时不生效.目前采用动画列表最末尾的那个动画作为当前动画)
    /// </summary>
    public int priority;

    /// <summary>
    /// 动画持续时间(这个并不重要,但是最好还是记录.表现层有用)
    /// </summary>
    public float duringTime;
    
    /// <summary>
    /// 开始时间
    /// </summary>
    public float startTime;
}

/// <summary>
/// 角色额外的移动信息
/// </summary>
public struct RoleMoveInfo
{
    /// <summary>
    /// 方向
    /// </summary>
    public Vector3 dir;

    /// <summary>
    /// 持续时间
    /// </summary>
    public float duringTime;
}