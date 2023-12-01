using UnityEngine;

public class RoleInput : MonoBehaviour
{
    private Role role;

    private float preTime;

    private void Awake()
    {
        role = GetComponent<Role>();
    }

    void Update()
    {
        // 获取输入的方向（水平和垂直轴）
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        var target = new Vector3(inputX, 0, inputZ);
        role.SetMoveDir(target);
        var keyJ = Input.GetKey(KeyCode.J);
        var keyK = Input.GetKey(KeyCode.K);
        if (keyJ && Time.time - preTime > 1)
        {
            preTime = Time.time;
            role.animator.ResetTrigger("attack");
            role.animator.SetTrigger("attack");
        }
    }
}