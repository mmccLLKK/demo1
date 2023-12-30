using System;
using System.Collections.Generic;

public class RoleFSM : StateMachineManager<RoleStateType>
{
    /// <summary>
    /// 移动回调
    /// </summary>
    private List<Action> onMoveCBs = new();
}