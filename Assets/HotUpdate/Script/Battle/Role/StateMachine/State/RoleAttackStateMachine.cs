public class RoleAttackStateMachine : RoleStateMachineBase
{
    public override void OnEntry()
    {
        role.playActoin("attack");
    }

    public override void tick()
    {
    }

    public override void OnLeave()
    {
    }
}