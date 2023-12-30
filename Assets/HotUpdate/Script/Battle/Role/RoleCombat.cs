using UnityEngine;

/// <summary>
/// 角色连招管理器
/// </summary>
public class RoleCombat : MonoBehaviour
{
    protected RoleAbility roleAbility;

    protected RoleFSM roleFsm;

    public void Init()
    {
        roleAbility = gameObject.GetComponent<RoleAbility>();
        roleFsm = gameObject.GetComponent<RoleFSM>();
    }
}