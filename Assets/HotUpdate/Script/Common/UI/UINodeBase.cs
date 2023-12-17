using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class UINodeBase
{
    public GameObject gameObject;

    public Transform transform;

    public RectTransform rectTransform;

    protected List<UINodeBase> childs;

    public void Init(GameObject gameObject)
    {
        this.gameObject = gameObject;
        this.transform = gameObject.transform;
        this.rectTransform = gameObject.GetComponent<RectTransform>();
        this.OnInit();
    }

    /// <summary>
    /// 添加一个ui子节点
    /// </summary>
    /// <param name="childGameObject">游戏物体</param>
    /// <typeparam name="T">类型</typeparam>
    public void AddChild<T>(GameObject childGameObject) where T : UINodeBase
    {
        if (!childGameObject)
        {
            throw new Exception("UI子节点为空");
        }

        var instance = Activator.CreateInstance<T>();
        instance.Init(childGameObject);
        instance.OnInit();
    }

    public void SetActive(bool isActive)
    {
        this.gameObject.SetActive(isActive);
    }

    // /// <summary>
    // /// 移除一个ui子节点暂时不太好控制
    // /// </summary>
    // public void RemoveChild(UINodeBase uiNodeBase)
    // {
    //     var findIndex = childs.Find(node => uiNodeBase == node);
    //     if (findIndex == null)
    //     {
    //         Debug.LogError("想要移除的UI子节点不存在");
    //         return;
    //     }
    //     childs.Remove(findIndex);
    //     findIndex.OnDestroy();
    // }

    /// <summary>
    /// 逻辑上的销毁
    /// TIPS: 目前不会有资源上的销毁.因为资源的销毁依赖于资源管理器.最好交由外部管理
    /// </summary>
    public void Destory()
    {
        //通知子组件销毁的消息
        if (childs != null)
        {
            foreach (var uiNodeBase in childs)
            {
                uiNodeBase.Destory();
            }
        }

        this.OnDestroy();
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