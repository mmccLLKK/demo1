using UnityEngine;


/// <summary>
/// 角色动画
/// </summary>
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