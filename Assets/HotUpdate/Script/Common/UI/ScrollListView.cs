using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 无限滚动列表
/// 子物体锚左上角
/// </summary>
public class ScrollListView : UINodeBase
{
    protected ScrollRect scrollRect;

    protected ScrollListViewAdapter<object> adapter;

    /// <summary>
    /// 挂载点
    /// </summary>
    protected RectTransform content;

    /// <summary>
    /// 统一都这么大.可变大小的有点难搞
    /// </summary>
    protected Vector2 childSize;

    /// <summary>
    /// mask 区域只有在这个区域内的组件才需要显示
    /// </summary>
    protected Rect maskRect;

    /// <summary>
    /// 显示中的物体
    /// </summary>
    protected List<ScrollListViewItem<object>> items = new();

    /// <summary>
    /// 未使用的部分
    /// </summary>
    protected List<ScrollListViewItem<object>> poolItems = new();

    protected void OnScrollIng(Vector2 value)
    {
        //该回收的回收.

        //该创建的创建
    }

    public override void OnInit()
    {
        scrollRect = this.gameObject.GetComponent<ScrollRect>();
        scrollRect.onValueChanged.AddListener(value => { OnScrollIng(value); });
        this.content = scrollRect.content;
        //初始化锚点
        this.rectTransform.anchorMax = new Vector2(0, 1);
        this.rectTransform.anchorMin = new Vector2(0, 1);
        this.rectTransform.anchoredPosition = Vector2.zero;
    }
    
    /// <summary>
    /// 设置无线滚动列表
    /// </summary>
    /// <param name="adapter"></param>
    /// <typeparam name="T"></typeparam>
    public void SetAdapter<T>(ScrollListViewAdapter<T> adapter)
    {
        this.adapter = (ScrollListViewAdapter<object>) adapter;
        //确定大小
        var item = this.adapter.GetItem();
        item.rectTransform.SetParent(this.transform);
        var childRect = item.rectTransform.rect;
        // 子物体大小
        this.childSize = childRect.size;
        this.poolItems.Add(item);
    }

    /// <summary>
    /// 刷新
    /// </summary>
    public void Refresh()
    {
        var dataCount = this.adapter.GetDataCount();

        for (int index = 0; index < dataCount; index++)
        {
            var data = this.adapter.GetData(index);
        }
    }

    /// <summary>
    /// 获取一个新的Item
    /// </summary>
    /// <returns></returns>
    protected ScrollListViewItem<object> GetFreeItem()
    {
        ScrollListViewItem<object> item = null;

        if (poolItems.Count > 0)
        {
            item = poolItems.Pop();
        }
        else
        {
            item = this.adapter.GetItem();
        }

        return item;
    }

    /// <summary>
    /// 回收
    /// </summary>
    /// <param name="item"></param>
    protected void RecycleItem(ScrollListViewItem<object> item)
    {
        //直接放这里也行
        item.gameObject.SetActive(false);
        this.items.Add(item);
    }

    public override void OnDestroy()
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// 循环列表子组件
/// </summary>
public abstract class ScrollListViewItem<T> : UINodeBase
{
    protected T data;

    public void SetData(T t)
    {
        this.data = t;
    }
}

/// <summary>
///  滚动列表适配器
/// </summary>
public interface ScrollListViewAdapter<T>
{
    /// <summary>
    /// 获取一个新的物体实例
    /// </summary>
    public ScrollListViewItem<T> GetItem();

    /// <summary>
    /// 获取数据
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public T GetData(int index);

    /// <summary>
    /// 获取数据个数
    /// </summary>
    public int GetDataCount();
}