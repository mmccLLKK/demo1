using System;
using UnityEngine;

public abstract class UIBase
{
    public UIInfo uiInfo;

    public GameObject gameObject;

    protected Transform transform;

    protected RectTransform rectTransform;

    protected Canvas canvas;

    /// <summary>
    /// 关闭UI
    /// </summary>
    public Action CloseUI;

    public void Init(GameObject gameObject)
    {
        this.gameObject = gameObject;
        this.transform = gameObject.transform;
        this.rectTransform = gameObject.GetComponent<RectTransform>();
    }

    public abstract void OnInit();

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public abstract void OnClose();
}