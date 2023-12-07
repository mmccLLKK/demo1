using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachineManager<T> : MonoBehaviour
{
    /// <summary>
    /// 状态转移关系
    /// </summary>
    protected Dictionary<T, HashSet<T>> transferDic = new Dictionary<T, HashSet<T>>();

    /// <summary>
    /// 所有状态
    /// </summary>
    protected Dictionary<T, StateMachineBase> statesDic = new Dictionary<T, StateMachineBase>();

    protected T curStateType;

    /// <summary>
    /// 设置所有状态
    /// </summary>
    public void SetStatesInfo(List<(T, StateMachineBase)> states, List<(T, T)> transfer)
    {
        this.statesDic.Clear();
        this.transferDic.Clear();

        //状态转移情况记录
        foreach (var valueTuple in transfer)
        {
            var (from, to) = valueTuple;
            if (!this.transferDic.ContainsKey(from))
            {
                this.transferDic.Add(from, new HashSet<T>());
            }

            this.transferDic[from].Add(to);
        }

        foreach (var valueTuple in states)
        {
            var (type, state) = valueTuple;
            this.statesDic.Add(type, state);
        }
    }

    /// <summary>
    /// 设置状态
    /// </summary>
    public void SetState(T stateType, bool isForce = false)
    {
        //不是force才需要走这些判断
        if (!isForce)
        {
            if (!this.transferDic.ContainsKey(this.curStateType))
            {
                Debug.Log("未找到对应的状态:" + stateType);
                return;
            }

            if (!this.transferDic[this.curStateType].Contains(stateType))
            {
                Debug.Log($"状态:{this.curStateType}不能转移到状态:{stateType}");
            }
        }

        var curState = this.statesDic[this.curStateType];
        var nexState = this.statesDic[stateType];
        curState?.OnLeave();
        nexState?.OnEntry();
        
        //改变状态
        this.curStateType = stateType;
    }

    /// <summary>
    /// 判断是否处于状态
    /// </summary>
    public bool IsInState(T stateType)
    {
        return this.curStateType.Equals(stateType);
    }

    public void Update()
    {
        //状态Tick
        var stateMachineBase = this.statesDic[this.curStateType];
        stateMachineBase?.tick();
    }
}