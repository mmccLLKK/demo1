using System;
using System.Collections.Generic;
using Cinemachine;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// TODO 后续使用命令系统替代(目前暂时不作处理)
/// </summary>
public class RoleInput : MonoBehaviour
{
    protected RoleCtrl roleCtrl;

    private CinemachineFreeLook cinemachineFreeLook;

    public float camYAxisMaxSpeed = 8;

    public float camXAxisMaxSpeed = 400;

    /// <summary>
    /// 输入分发(一个简单的控制器切换逻辑)
    /// </summary>
    protected Dictionary<CursorLockMode, Action> inputDispatch = new();

    private void Awake()
    {
        roleCtrl = GetComponent<RoleCtrl>();

        // 锁定鼠标光标
        Cursor.lockState = CursorLockMode.Locked;

        //获取自由相机
        cinemachineFreeLook = this.gameObject.GetComponentInChildren<CinemachineFreeLook>();

        inputDispatch.Add(CursorLockMode.None, () => { this.UpdateOnNoneMode(); });
        inputDispatch.Add(CursorLockMode.Locked, () => { this.UpdateOnLockMode(); });
    }

    private void ChangeMouseMode(CursorLockMode mode)
    {
        if (CursorLockMode.Locked == mode)
        {
            cinemachineFreeLook.m_YAxis.m_MaxSpeed = camYAxisMaxSpeed;
            cinemachineFreeLook.m_XAxis.m_MaxSpeed = camXAxisMaxSpeed;
        }
        else
        {
            //其他模式的镜头是不让转的
            cinemachineFreeLook.m_YAxis.m_MaxSpeed = 0;
            cinemachineFreeLook.m_XAxis.m_MaxSpeed = 0;
        }

        var frameCount = Time.frameCount;
        Cursor.lockState = mode;
        Debug.Log($"{frameCount} 切换. {mode}");
    }

    protected bool CheckIsMouseClickOnUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    protected void UpdateOnLockMode()
    {
        // 获取输入的方向（水平和垂直轴）
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        var target = new Vector3(inputX, 0, inputZ);

        // TODO 换成全局处理.这里临时纠正前进方向
        var main = Camera.main;
        // 把输入当作局部坐标处理.然后转换到时间空间
        target = main.transform.TransformVector(target);

        //这里不处理平面投影
        roleCtrl.targetDir = target;

        // 按下 Escape 键释放鼠标光标
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            ChangeMouseMode(CursorLockMode.None);
        }
    }

    protected void UpdateOnNoneMode()
    {
        // 按下 Escape 切换状态
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            ChangeMouseMode(CursorLockMode.Locked);
        }

        // 点击空白区域也会进入到锁定状态
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            var checkIsMouseClickOnUI = CheckIsMouseClickOnUI();
            if (!checkIsMouseClickOnUI)
            {
                ChangeMouseMode(CursorLockMode.Locked);
            }
        }
    }

    protected void Update()
    {
        var valueOrDefault = inputDispatch.GetValueOrDefault(Cursor.lockState, null);
        valueOrDefault?.Invoke();
    }
}