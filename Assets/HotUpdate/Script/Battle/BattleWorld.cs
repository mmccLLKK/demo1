using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 战场世界
/// </summary>
public class BattleWorld : MonoBehaviour
{
    protected UIMap _uiMap;
    protected UIButtom _buttom;

    public BornPoint roleBornPoint;

    #region 开发中应该使用渐进式的设计.当此处庞大到无法承载的时候再进行系统的抽取.除非你经验丰富,能设计出一整套完整接口

    /// <summary>
    /// 角色字典
    /// </summary>
    protected Dictionary<int, Role> _roles = new();

    /// <summary>
    /// 玩家绑定角色字典
    /// </summary>
    protected Dictionary<string, int> _playerToRole = new();

    /// <summary>
    /// 角色到玩家的绑定
    /// </summary>
    protected Dictionary<int, string> _roleToPlayer = new();

    #endregion


    private void Start()
    {
        roleBornPoint = GetComponentInChildren<BornPoint>(true);
    }

    public async UniTask Init()
    {
    }

    public async UniTask InitUI()
    {
        _uiMap = await UIManager.Inst().OpenUI("UI_MAP") as UIMap;
        _buttom = await UIManager.Inst().OpenUI("UI_BUTTOM") as UIButtom;
    }

    /// <summary>
    /// 站场内添加角色
    /// </summary>
    /// <returns></returns>
    public async UniTask<Role> CreateRole(RoleData roleData)
    {
        //TODO 本来应该从 playerData 中拿到玩家数据初始化角色的.但是现在没有角色存档系统
        //TODO 接入真实的属性系统
        //TODO 接入真实的技能系统

        return null;
    }

    /// <summary>
    /// 玩家绑定角色
    /// </summary>
    public void PlayerBindRole(string playerId, int roleInstId)
    {
        
    }

    /// <summary>
    /// 设置当前操作角色
    /// </summary>
    public void SetCurRole()
    {
        //简单处理. 分发器都不用
    }

    private void OnDestroy()
    {
        _uiMap?.CloseUI();
        _buttom?.CloseUI();
    }
}