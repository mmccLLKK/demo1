using System;
using UnityEngine;

public class AIFSM : StateMachineManager<AIStateType>
{
    /// <summary>
    /// 外部要对状态机设置参数.
    /// </summary>
    public StateMachineBase GetStateInst(AIStateType stateType)
    {
        return statesDic[stateType];
    }

    public void ChangeToTrack(GameObject enemy)
    {

    }

}