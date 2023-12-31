public class RoleMoveStateMachine : RoleStateMachineBase
{
    protected AnimInfo animInfo;

    public override void OnEntry()
    {
        animInfo = role.roleAnim.Play("move", -1, 0.1f);
    }

    public override void tick()
    {
        //速度.目前和动画还没匹配归一化
        float moveSpeed = role.kcc.velocity.normalized.magnitude;
        role.roleAnim.animator.SetFloat("moveSpeed", moveSpeed);
    }

    public override void OnLeave()
    {
        role.roleAnim.RemovePlay(animInfo);
    }
}