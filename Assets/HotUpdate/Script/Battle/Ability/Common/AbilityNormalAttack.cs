/// <summary>
/// 这里使用简单配置来代替通用配置的作用. 如果是timeline方式的配置 应该是 配置节点即可.
/// </summary>
[AbilityConfig(templateName: "normal_attack", templateName = "通用普通攻击配置")]
public class AbilityNormalAttackConfig : AbilityConfigBase
{
    /// <summary>
    /// 持续时间
    /// </summary>
    public float duringTime = 0;

    /// <summary>
    /// 可退出时间
    /// </summary>
    public float exitTime = 0;

    /// <summary>
    /// 动画名
    /// </summary>
    public string animName = null;

    /// <summary>
    /// 很多单段普攻都可以有一点点位移.为了这个准备的
    /// </summary>
    public float moveSpeed = 0;
}

[AbilityType(templateName: "normal_attack", templateName = "通用普通攻击")]
public class AbilityNormalAttack : AbilityBase
{
    protected AbilityNormalAttackConfig Config()
    {
        return this.configBase as AbilityNormalAttackConfig;
    }

    public override void CasteStart()
    {
        base.CasteStart();
        var config = this.Config();
        this.timeMax = config.duringTime;
        this.timeProcess = 0;
        this.timeDuring = 0;
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);
    }
}