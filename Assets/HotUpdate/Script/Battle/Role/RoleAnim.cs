using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 角色动画
/// </summary>
public class RoleAnim : MonoBehaviour
{
    public Animator animator;

    protected int animaId = 100;

    /// <summary>
    /// 当前运行中的动画
    /// </summary>
    protected List<AnimInfo> animInfos = new();

    /// <summary>
    /// 当前正在播放的
    /// </summary>
    protected AnimInfo curAnimInfo = null;

    /// <summary>
    /// 播放一个动画
    /// </summary>
    /// <param name="animName"></param>
    /// <param name="cross"></param>
    public AnimInfo Play(string animName, float duringTime, float cross)
    {
        var info = new AnimInfo()
        {
            id = this.animaId++,
            animName = animName,
            duringTime = duringTime,
            startTime = Time.time,
        };
        this.animator.CrossFade(animName, cross, -1);

        return info;
    }

    /// <summary>
    /// 动画取消,移除播放
    /// </summary>
    /// <param name="animInfo"></param>
    public void RemovePlay(AnimInfo animInfo)
    {
        this.animInfos.Remove(animInfo);
    }


    /// <summary>
    /// 避免GC
    /// </summary>
    protected List<AnimInfo> needRemove = new();

    private void FixedUpdate()
    {
        // 下边的逻辑比较耗费时间,可以通过预处理的方法来优化
        needRemove.Clear();
        var curTime = Time.time;
        foreach (var animInfo in animInfos)
        {
            //这种是无限持续时间的
            if (animInfo.duringTime < 0)
            {
                continue;
            }

            var endTime = animInfo.startTime + animInfo.duringTime;
            if (endTime < curTime)
            {
                needRemove.Add(animInfo);
            }
        }

        // 移除应该移除的动画
        foreach (var animInfo in needRemove)
        {
            animInfos.Remove(animInfo);
        }

        var last = animInfos.Back();
        if (last == null || curAnimInfo == last)
        {
            return;
        }

        curAnimInfo = last;

        //播放的时候层和过渡时间都使用默认值
        this.animator.CrossFade(curAnimInfo.animName, 0.1f, -1);
    }
}