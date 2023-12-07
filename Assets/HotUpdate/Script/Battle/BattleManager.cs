using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    protected UIMap _uiMap;
    protected UIButtom _buttom;

    public BornPoint roleBornPoint;

    #region 开发中应该使用渐进式的设计.当此处庞大到无法承载的时候再进行系统的抽取.除非你经验丰富,能设计出一整套完整接口

    protected Dictionary<int, Role> _roles = new();

    #endregion


    private void Start()
    {
        roleBornPoint = GetComponentInChildren<BornPoint>(true);
    }

    public async void Init()
    {
    }

    protected async UniTask InitUI()
    {
        _uiMap = await UIManager.Inst().OpenUI("UI_MAP") as UIMap;
        _buttom = await UIManager.Inst().OpenUI("UI_BUTTOM") as UIButtom;
    }

    /// <summary>
    /// 站场内添加角色
    /// </summary>
    /// <returns></returns>
    public async UniTask<Role> AddRole(RoleConfig roleConfig)
    {
        
        
        
        
        return null;
    }

    /// <summary>
    /// 设置角色去
    /// </summary>
    /// <param name="role"></param>
    public void SetRoleToBorn(Role role)
    {
    }


    private void OnDestroy()
    {
        _uiMap?.CloseUI();
        _buttom?.CloseUI();
    }
}