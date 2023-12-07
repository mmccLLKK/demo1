using UnityEngine;

public class TrackStateMachine : AIStateMachineBase
{
    public GameObject enemy;

    public override void OnEntry()
    {
        Debug.Log("进入跟踪");
    }

    public override void tick()
    {
        if (!role || !enemy)
        {
            return;
        }

        var dir = enemy.transform.position - role.transform.position;
        role.SetMoveDir(dir);
    }

    public override void OnLeave()
    {
    }
}