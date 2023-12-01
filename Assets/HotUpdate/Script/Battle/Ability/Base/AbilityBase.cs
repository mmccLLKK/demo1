using UnityEngine;

public abstract class AbilityBase
{
    public Role owner;
    public Vector3 dir = Vector3.zero;

    public void SetDir(Vector3 dir)
    {
        this.dir = dir;
    }

    public void SetOwner(Role role)
    {
    }

    /// <summary>
    /// 释放开始
    /// </summary>
    public void CasteStart()
    {
    }

    /// <summary>
    /// 技能释放完成
    /// </summary>
    public void CasteEnd()
    {
    }
}