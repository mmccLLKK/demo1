public class RoleIdleStateMachine : RoleStateMachineBase
{
    public override void OnEntry()
    {
        role.playActoin("idle");
    }

    public override void tick()
    {
    }

    public override void OnLeave()
    {
    }
}