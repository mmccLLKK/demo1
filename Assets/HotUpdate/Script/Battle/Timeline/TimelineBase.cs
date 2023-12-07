using System;
using UnityEngine;

namespace Game.Mine
{
    /// <summary>
    /// Timeline 轨道
    /// </summary>
    public enum TimelineTraceType
    {
        Anim, //动画播放
        Pos, //位置改变
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
    public abstract class TimelineBase
    {
        protected GameObject gameObject;

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

        public virtual void Init(GameObject gameObject)
        {
            this.gameObject = gameObject;
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


    /// <summary>
    /// 动画轨道(播放动画使用)
    /// </summary>
    [Timeline("播放动画", TimelineTraceType.Anim)]
    public class TimelineAnim : TimelineBase
    {
        protected Animator animator;

        /// <summary>
        /// 动画名
        /// </summary>
        public string animName;

        public override void Init(GameObject gameObject)
        {
            base.Init(gameObject);
            if (!gameObject)
            {
                return;
            }

            animator = gameObject.GetComponent<Animator>();
        }

        public override void Start()
        {
        }

        public override void Tick()
        {
        }
    }

    [Timeline("位置调整", TimelineTraceType.Pos)]
    public class TimelinePos : TimelineBase
    {
        public override void Start()
        {
        }

        public override void Tick()
        {
        }
    }
}