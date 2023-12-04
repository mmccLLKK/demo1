using System.Collections.Generic;
using UnityEngine;

public class Role : MonoBehaviour
{
    public Animator animator;

    public RoleFSM roleFsm;

    public Timer timer;

    #region 角色逻辑状态动态数据

    /// <summary>
    /// 是否能移动
    /// 状态标志位 (多数使用GAS这套的逻辑都会有这种标志位)
    /// </summary>
    public bool canMove = true;

    /// <summary>
    /// 是否能转身
    /// </summary>
    public bool canRotate = true;

    /// <summary>
    /// 是否能释放技能
    /// </summary>
    public bool canAttack = true;

    #endregion

    /// <summary>
    /// 期望的移动方向(也可以是目标点)
    /// </summary>
    public Vector3 moveDir = Vector3.zero;

    #region 角色属性

    /// <summary>
    /// 移动速度
    /// </summary>
    public float moveSpeed = 5f; // 移动速度 

    /// <summary>
    /// 阻尼
    /// </summary>
    public float dragValue = 1f;

    /// <summary>
    /// 跳跃力
    /// </summary>
    public float jumpForce = 3f;

    #endregion

    #region 角色状态控制变量

    /// <summary>
    /// 是否处于地上
    /// </summary>
    public bool isOnGround = true;

    /// <summary>
    /// 是否在空中
    /// </summary>
    public bool isInAir = false;

    /// <summary>
    /// 地面法线
    /// </summary>
    public Vector3 groundNormal;

    /// <summary>
    /// 重力以及方向
    /// </summary>
    public Vector3 gravityDir;

    /// <summary>
    /// 重力缩放
    /// </summary>
    public float gravityScale = 1f;

    #endregion

    private void Awake()
    {
        //一般角色状态机都是通过配置来的.我们这里直接写死在里边
        List<(RoleStateType, StateMachineBase)> states = new List<(RoleStateType, StateMachineBase)>();
        states.Add((RoleStateType.idle, new RoleIdleStateMachine()));
        states.Add((RoleStateType.move, new RoleMoveStateMachine()));
        // states.Add((RoleStateType.attack, new RoleAttackStateMachine()));

        //初始化赋值Role
        foreach (var valueTuple in states)
        {
            valueTuple.Item2.role = this;
        }

        // 因为逻辑简单...这里居然是一个笛卡尔积
        List<(RoleStateType, RoleStateType)> trans = new List<(RoleStateType, RoleStateType)>();
        trans.Add((RoleStateType.idle, RoleStateType.move));
        // trans.Add((RoleStateType.idle, RoleStateType.attack));
        // trans.Add((RoleStateType.move, RoleStateType.attack));
        trans.Add((RoleStateType.move, RoleStateType.idle));
        // trans.Add((RoleStateType.attack, RoleStateType.idle));
        // trans.Add((RoleStateType.attack, RoleStateType.move));

        //设置状态机数据
        roleFsm.SetStatesInfo(states, trans);

        roleFsm.SetState(RoleStateType.idle, true);
    }

    //walk 特效
    public void playActoin(string anim)
    {
        //播放动效
        this.animator.CrossFade(anim, 0.1f);

        //播放特效

        //播放音效
    }

    public void SetMoveDir(Vector3 dir)
    {
        //方向修正,先这类游戏输入方向一定是在世界方向上方的投影
        dir = Vector3.ProjectOnPlane(dir, Vector3.up);
        moveDir = dir;
    }

    protected void Move()
    {
        if (!canMove || moveDir == Vector3.zero)
        {
            return;
        }

        var dir = moveDir.normalized;
        if (canRotate)
        {
            this.transform.forward = Vector3.Slerp(this.transform.forward, dir, 0.5f);
        }

        this.transform.position += Time.deltaTime * moveSpeed * dir;
    }

    public void ChangeState()
    {
        if (roleFsm.IsInState(RoleStateType.attack))
        {
            return;
        }

        var dir = moveDir;
        // 切换角色状态 (如果期望是要移动,但是又不处于移动状态.)
        if (dir.magnitude > 0 && !roleFsm.IsInState(RoleStateType.move))
        {
            roleFsm.SetState(RoleStateType.move);
        }

        // 同理切换到站立状态
        if (dir.magnitude == 0 && !roleFsm.IsInState(RoleStateType.idle))
        {
            roleFsm.SetState(RoleStateType.idle);
        }
    }

    public void Update()
    {
        //改变默认状态
        ChangeState();
        //移动
        Move();
    }

    /// <summary>
    /// 3d 物理游戏.一般的状态都是在 fixedupdate 里边计算
    /// </summary>
    public void FixedUpdate()
    {
        //检查是否在地面
        CheckGround();
        //纠正站立方向
        CorrectUpDir();
    }

    /// <summary>
    /// 检查是否在地面
    /// </summary>
    protected void CheckGround()
    {
        isOnGround = false;
        isInAir = false;
        Vector3 normal = Vector3.zero;
        var raycastHits = new RaycastHit[10];
        var hitCount = Physics.RaycastNonAlloc(this.transform.position, this.gravityDir, raycastHits, this.gravityDir.sqrMagnitude);
        for (int index = 0; index < hitCount; index++)
        {
            //确定地面法线
            var raycastHit = raycastHits[index];
        }

        // 如果没有地面法线.那么默认处理为角色的up方向
        if (normal == Vector3.zero)
        {
            isOnGround = false;
            isInAir = true;
            normal = -(this.gravityDir.normalized);
        }

        groundNormal = normal;
    }

    /// <summary>
    /// 纠正站立方向
    /// </summary>
    protected void CorrectUpDir()
    {
    }
}