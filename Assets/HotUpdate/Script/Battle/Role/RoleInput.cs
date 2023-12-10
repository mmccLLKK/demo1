using UnityEngine;

/// <summary>
/// TODO 后续使用命令系统替代(目前暂时不作处理)
/// </summary>
public class RoleInput : MonoBehaviour
{
    private Role role;

    private float preTime;

    private void Awake()
    {
        role = GetComponent<Role>();

        // 锁定鼠标光标
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // 获取输入的方向（水平和垂直轴）
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        var target = new Vector3(inputX, 0, inputZ);

        // TODO 换成全局处理.这里临时纠正前进方向
        var main = Camera.main;
        // 把输入当作局部坐标处理.然后转换到时间空间
        target = main.transform.TransformVector(target);

        role.SetMoveDir(target);
        var keyJ = Input.GetKeyDown(KeyCode.J);
        var keyK = Input.GetKeyDown(KeyCode.K);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            var allAbilities = role.roleAbilityManager.GetAllAbilities();
            var abilityBase = allAbilities[0];
            if (abilityBase.abilityStatus == AbilityStatus.CanRelease)
            {
                abilityBase.CasteStart();
            }
        }

        // 按下 Escape 键释放鼠标光标
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}