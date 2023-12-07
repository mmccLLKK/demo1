public abstract class StateMachineBase
{
    public Role role;

    public abstract void OnEntry();

    public abstract void tick();

    public abstract void OnLeave();
}