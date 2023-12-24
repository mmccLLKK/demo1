using UnityEngine;

/// <summary>
/// 动画信息
/// </summary>
public struct AnimInfo
{
    /// <summary>
    /// 动画名
    /// </summary>
    public string animName;

    /// <summary>
    /// 优先级
    /// </summary>
    public int priority;

    /// <summary>
    /// 动画持续时间(这个并不重要,但是最好还是记录.表现层有用)
    /// </summary>
    public float duringTime;
}

public class RoleAnim : MonoBehaviour
{
    public Animator animator;

    /// <summary>
    /// 播放一个动画
    /// </summary>
    /// <param name="animName"></param>
    /// <param name="cross"></param>
    public void Play(string animName, float cross)
    {
        this.animator.CrossFade(animName, cross);
    }

    public void Play(AnimInfo animInfo)
    {
    }


    private void FixedUpdate()
    {
        var fixedDeltaTime = Time.fixedDeltaTime;
    }
}