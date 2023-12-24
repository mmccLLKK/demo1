using System;
using System.Collections.Generic;
using UnityEngine;

public class Role : MonoBehaviour
{
    public RoleAnim roleAnim;

    public RoleFSM roleFsm;

    public RoleAbilityManager abilityManager;

    /// <summary>
    /// 快速开发的时候暂时使用已有的 KCC 代替
    /// </summary>
    public CharacterController kcc;

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

    #endregion

    /// <summary>
    /// 期望的移动方向(也可以是目标点)
    /// </summary>
    public Vector3 moveDir = Vector3.zero;

    /// <summary>
    /// 当前的速度
    /// </summary>
    public Vector3 curVec = Vector3.zero;

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
    /// 重量
    /// </summary>
    public float weight = 70;

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
    public Vector3 gravityDir = Vector3.down;

    /// <summary>
    /// 重力缩放
    /// </summary>
    public float gravityScale = 1f;

    #endregion

    /// <summary>
    /// 为了让这个体系在空编辑器下也能使用,后续会去掉该流程
    /// </summary>
    protected bool _is_init = false;

    public void Init()
    {
        if (_is_init)
        {
            return;
        }

        _is_init = true;

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

        //技能管理器
        abilityManager = this.gameObject.GetComponent<RoleAbilityManager>();
    }

    private void Awake()
    {
        Init();
    }

    //walk 特效
    public void playActoin(string anim)
    {
        //播放动效
        this.roleAnim?.Play(anim, 0.1f);

        //播放特效

        //播放音效
    }

    public void SetMoveDir(Vector3 dir)
    {
        //方向修正,先这类游戏输入方向一定是在世界方向上方的投影
        dir = Vector3.ProjectOnPlane(dir, Vector3.up);
        moveDir = dir;
    }

    /// <summary>
    /// 计算当前速度
    /// </summary>
    /// <returns></returns>
    protected Vector3 CalCurSpeed()
    {
        //这里的计算方式应该根据当前的游戏类型考虑
        Vector3 rst = Vector3.zero;
        if (canMove)
        {
            rst += moveDir * moveSpeed;
        }

        // rst += Vector3.up * 19.8f;
        return rst;
    }

    /// <summary>
    /// 移动
    /// </summary>
    protected void Movement()
    {
        var calCurSpeed = CalCurSpeed();

        // 转向
        if (canRotate)
        {
            var dir = calCurSpeed.normalized;
            var projectOnPlane = Vector3.ProjectOnPlane(dir, Vector3.up);
            this.transform.forward = Vector3.Slerp(this.transform.forward, projectOnPlane, 0.5f);
        }

        //直接修改位置的做法(如果后续有需求.可以处理这部分的东西)
        kcc.Move(calCurSpeed * Time.deltaTime);
    }

    protected void Jump()
    {
        //没有jump动画.还是别跳了
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

        //画出debug信息
        DrawDebug();
    }

    private void OnDrawGizmos()
    {
        //移动方向debug
        Gizmos.DrawRay(transform.position, transform.forward);
    }

    protected void DrawDebug()
    {
        Debug.DrawRay(transform.position, kcc.velocity, Color.green);
    }

    /// <summary>
    /// 3d 物理游戏.一般的状态都是在 fixedupdate 里边计算
    /// </summary>
    public void FixedUpdate()
    {
        //检查是否在地面
        CheckGround();
        //跳跃
        Jump();
        //移动
        Movement();

        curVec = kcc.velocity;
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
        var hitCount = Physics.RaycastNonAlloc(this.transform.position, this.gravityDir, raycastHits,
            this.gravityDir.sqrMagnitude);
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
}