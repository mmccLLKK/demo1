using UnityEngine;

/// <summary>
/// 攻击AI状态
/// </summary>
public class StrikeStateMachine : AIStateMachineBase
{
    public float entryTime;

    public float exstTime = 3f;


    public override void OnEntry()
    {
        entryTime = Time.time;
        // role.Attack(AbilityType.attack1);
    }

    public override void tick()
    {
        //挥砍过后AI愣住...等待一定时间.再考虑下一次挥砍
        if (Time.time > entryTime + exstTime)
        {
            aifsm.SetState(AIStateType.track);
        }
    }

    public override void OnLeave()
    {
    }
}