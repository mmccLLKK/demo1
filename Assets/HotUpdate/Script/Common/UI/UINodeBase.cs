using UnityEngine;

public abstract class UINodeBase
{
    public GameObject gameObject;

    protected Transform transform;

    protected RectTransform rectTransform;

    public void Init(GameObject gameObject)
    {
        this.gameObject = gameObject;
        this.transform = gameObject.transform;
        this.rectTransform = gameObject.GetComponent<RectTransform>();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public abstract void OnInit();

    /// <summary>
    /// 销毁时
    /// </summary>
    public abstract void OnDestroy();
}