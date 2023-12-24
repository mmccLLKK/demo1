using System;
using UnityEngine;

namespace Game.Mine
{
    /// <summary>
    /// Timeline 轨道
    /// </summary>
    public enum TimelineTraceType
    {
        Main, //主时间轴
        Anim, //动画播放
        Movement, //移动(根据速度)
        DamageArea, //伤害区域
        CharacterStateCtrl, //角色状态控制
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class TimelineAttribute : Attribute
    {
        /// <summary>
        /// Timeline 类型名称
        /// </summary>
        public string name { get; set; }

        public TimelineTraceType traceType { get; set; }

        public TimelineAttribute(string name, TimelineTraceType traceType)
        {
        }
    }

    /// <summary>
    /// 基础timeline
    /// </summary>
    public abstract class TimelineBase<T>
    {
        protected T target;

        /// <summary>
        /// 开始时间 (毫秒)
        /// </summary>
        public int startTimeMS;

        /// <summary>
        /// 持续时间
        /// </summary>
        public int lenTimeMS;

        /// <summary>
        /// 表现缩放
        /// </summary>
        public float scale;

        public virtual void Init(T target)
        {
            this.target = target;
        }

        /// <summary>
        /// 开始
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// 每一帧调用
        /// </summary>
        public abstract void Tick();
    }

    [Timeline("技能主时间轴", TimelineTraceType.Main)]
    public class TimelineAbilityMain : TimelineBase<bool>
    {
        public override void Start()
        {
            throw new NotImplementedException();
        }

        public override void Tick()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 动画轨道(播放动画使用)
    /// </summary>
    [Timeline("播放动画", TimelineTraceType.Anim)]
    public class TimelineAnim : TimelineBase<Animator>
    {
        /// <summary>
        /// 动画名
        /// </summary>
        public string animName;

        public override void Start()
        {
        }

        public override void Tick()
        {
        }
    }


    [Timeline("伤害区域", TimelineTraceType.DamageArea)]
    public class TimelineDamageArea : TimelineBase<int>
    {
        public override void Start()
        {
        }

        public override void Tick()
        {
        }
    }
}