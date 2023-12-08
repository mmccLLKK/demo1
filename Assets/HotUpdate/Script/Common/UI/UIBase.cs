using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class UIBase : UINodeBase
{
    public UIInfo uiInfo;

    protected Canvas canvas;

    /// <summary>
    /// 关闭UI
    /// </summary>
    public Action CloseUI;

    /// <summary>
    /// 播放显示动画
    /// </summary>
    /// <returns></returns>
    public async UniTask PlayShowAnim()
    {
    }

    /// <summary>
    /// 播放隐藏动画
    /// </summary>
    /// <returns></returns>
    public async UniTask PlayHideAnim()
    {
    }

    public void Show()
    {
        // this.gameObject.SetActive(true);
        this.canvas.enabled = true;
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
        this.canvas.enabled = false;
    }
}