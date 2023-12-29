public class RoleMoveStateMachine : RoleStateMachineBase
{
    public override void OnEntry()
    {
        role.playActoin("move");
    }

    public override void tick()
    {

    }

    public override void OnLeave()
    {
    }
}