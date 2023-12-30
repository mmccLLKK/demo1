using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角色控制管理器
/// </summary>
public class RoleCtrl : MonoBehaviour
{
    /// <summary>
    /// 期望的移动方向(以及速度)
    /// </summary>
    public Vector3 targetDir;

    /// <summary>
    /// 当前的角色控制状态
    /// </summary>
    public RoleCtrlStatus curRoleCtrlStatus;

    /// <summary>
    /// 角色控制状态叠加列表.
    /// </summary>
    protected List<RoleCtrlStatus> roleCtrlStatusList = new();

    /// <summary>
    /// 状态刷新
    /// </summary>
    protected RoleCtrlStatus RefreshRoleCtrlStatus()
    {
        var rst = RoleCtrlStatus.free;
        foreach (var cell in roleCtrlStatusList)
        {
            rst += cell;
        }

        return rst;
    }

    /// <summary>
    /// 给角色添加一个状态
    /// </summary>
    public RoleCtrlStatus AddRoleCtrlStatus(RoleCtrlStatus roleCtrlStatus)
    {
        roleCtrlStatusList.Add(roleCtrlStatus);
        curRoleCtrlStatus = this.RefreshRoleCtrlStatus();
        return curRoleCtrlStatus;
    }

    /// <summary>
    /// 移除一个角色控制状态
    /// </summary>
    public RoleCtrlStatus RemoveRoleCtrlStatus(RoleCtrlStatus roleCtrlStatus)
    {
        roleCtrlStatusList.Remove(roleCtrlStatus);
        curRoleCtrlStatus = this.RefreshRoleCtrlStatus();
        return curRoleCtrlStatus;
    }
}