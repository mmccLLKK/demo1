using UnityEngine;

/// <summary>
/// 巡逻AI状态
/// </summary>
public class PatrolStateMachine : AIStateMachineBase
{
    //我们巡逻想要到达的目标点
    public Vector3 targetPos = Vector3.zero;

    public override void OnEntry()
    {
        //进入逻辑
        Debug.Log("进入巡逻");
    }

    public override void tick()
    {
        return;
        //检测是否敌人在范围内
        Vector3 dis = role.transform.position - targetPos;
        //距离目标小于一米
        if (dis.magnitude < 1)
        {
            //随机下一个目标点
        }
        else
        {
            role.roleCtrl.targetDir = dis.normalized;
        }
    }

    public override void OnLeave()
    {
        //离开逻辑
    }
}