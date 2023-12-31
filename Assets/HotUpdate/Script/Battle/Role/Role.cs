using System.Collections.Generic;
using UnityEngine;

public class Role : MonoBehaviour
{
    public RoleAnim roleAnim;

    public RoleFSM roleFsm;

    public RoleAbility roleAbility;

    public RoleCombat roleCombat;

    public RoleCtrl roleCtrl;

    public RoleAttr roleAttr;

    public RoleModifier roleModifier;

    /// <summary>
    /// 快速开发的时候暂时使用已有的 KCC 代替
    /// </summary>
    public CharacterController kcc;

    #region 角色逻辑状态动态数据

    /// <summary>
    /// 额外速度
    /// </summary>
    public Vector3 extraVec;

    #endregion

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
        states.Add((RoleStateType.move, new RoleMoveStateMachine()));

        //初始化赋值Role
        foreach (var valueTuple in states)
        {
            valueTuple.Item2.role = this;
        }

        // 因为逻辑简单...这里居然是一个笛卡尔积
        List<(RoleStateType, RoleStateType)> trans = new List<(RoleStateType, RoleStateType)>();

        //设置状态机数据
        roleFsm.SetStatesInfo(states, trans);

        roleFsm.SetState(RoleStateType.move, true);

        //动画管理器
        roleAnim = this.GetComponent<RoleAnim>();
        if (!roleAnim)
        {
            roleAnim = this.gameObject.AddComponent<RoleAnim>();
        }

        roleCtrl.Init();
    }

    private void Awake()
    {
        Init();
    }

    /// <summary>
    /// 移动
    /// </summary>
    protected void Movement()
    {
        var targetDir = roleCtrl.targetDir;
        var ctrlStatus = roleCtrl.curRoleCtrlStatus;
        targetDir = Vector3.ProjectOnPlane(targetDir, Vector3.up);
        targetDir = targetDir * moveSpeed;


        // 转向
        if (ctrlStatus.canRotate)
        {
            var dir = targetDir.normalized;
            var projectOnPlane = Vector3.ProjectOnPlane(dir, Vector3.up);
            this.transform.forward = Vector3.Slerp(this.transform.forward, projectOnPlane, 0.5f);
        }

        // 如果不能移动,那么需要把移速降低
        if (!ctrlStatus.canMove)
        {
            targetDir = Vector3.zero;
        }
        // 一些额外的移动处理

        //直接修改位置的做法(如果后续有需求.可以处理这部分的东西)
        kcc.Move(targetDir * Time.deltaTime);
    }

    protected void Jump()
    {
        //没有jump动画.还是别跳了
    }

    public void Update()
    {
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