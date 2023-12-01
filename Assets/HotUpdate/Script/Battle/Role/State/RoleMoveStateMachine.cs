public class RoleMoveStateMachine : RoleStateMachineBase
{
    public override void OnEntry()
    {
        role.playActoin("walk");
    }

    public override void tick()
    {

    }

    public override void OnLeave()
    {
    }
}