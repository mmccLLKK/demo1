using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 排布方向
/// </summary>
public enum ScrollListViewDir
{
    horizaontal, //横向
    vertical, //竖向
}

/// <summary>
/// 配置参数
/// TODO 新增方向
/// </summary>
[Serializable]
public struct ScrollListViewParams
{
    /// <summary>
    /// 间距
    /// </summary>
    public float spaceing;

    /// <summary>
    /// 行/列个数
    /// </summary>
    public int num;

    /// <summary>
    /// 排布方向
    /// </summary>
    public ScrollListViewDir dir;
}

/// <summary>
/// 无限滚动列表
/// 子物体锚左上角
/// </summary>
public class ScrollListView : UINodeBase
{
    public ScrollRect scrollRect;

    protected ScrollListViewAdapter adapter;

    /// <summary>
    /// 参数
    /// </summary>
    protected ScrollListViewParams viewParams;

    /// <summary>
    /// 统一都这么大.可变大小的有点难搞
    /// </summary>
    protected Vector2 childSize;

    /// <summary>
    /// mask 区域只有在这个区域内的组件才需要显示
    /// </summary>
    public Rect maskRect;

    /// <summary>
    /// 显示中的物体
    /// key : index
    /// value : viewItem
    /// </summary>
    protected Dictionary<int, ScrollListViewItem> showingItemDic = new();

    /// <summary>
    /// 需要回收的(提前定义以免GC)
    /// </summary>
    protected List<int> needRecycle = new();

    /// <summary>
    /// 未使用的部分
    /// </summary>
    protected List<ScrollListViewItem> poolItems = new();

    /// <summary>
    /// 物体rect缓存 ()
    /// value : row col rect
    /// //TODO 使用这种方式有一个很明显的坏处.就是物体过多内存压力会变大.比如列表要显示上万个. 好处是可以使用Job加速
    /// </summary>
    protected Dictionary<int, (int, int, Rect)> itemRectDic = new();

    public override void OnInit()
    {
        scrollRect = this.gameObject.GetComponent<ScrollRect>();
        scrollRect.onValueChanged.AddListener(value => { OnScrollIng(value); });
        SetRectTransformNormal(scrollRect.viewport);
        SetRectTransformNormal(scrollRect.content);
        this.maskRect = ConvertRectTransformPositionToTarget(scrollRect.viewport, scrollRect.content, Camera.main);
        //初始化锚点
        SetRectTransformNormal(scrollRect.content);

        //设置初始化参数
        var viewParams = new ScrollListViewParams()
        {
            dir = ScrollListViewDir.vertical,
            num = 1,
            spaceing = 0f,
        };
        this.SetParams(viewParams);
        // this.RefreshAll();
    }

    public void SetRectTransformNormal(RectTransform rectTransform)
    {
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.pivot = new Vector2(0, 1);
        rectTransform.localScale = Vector3.one;
    }

    /// <summary>
    /// 设置参数(会刷新所有)
    /// </summary>
    public void SetParams(ScrollListViewParams viewParams)
    {
        this.viewParams = viewParams;
        // this.RefreshAll();
    }

    /// <summary>
    /// 设置无线滚动列表
    /// </summary>
    /// <param name="adapter"></param>
    /// <typeparam name="T"></typeparam>
    public void SetAdapter(ScrollListViewAdapter adapter)
    {
        this.adapter = adapter;
        //确定大小
        var item = this.GetFreeItem();
        var childRect = item.rectTransform.rect;
        // 子物体大小
        this.childSize = childRect.size;
        this.poolItems.Add(item);
    }

    protected void OnScrollIng(Vector2 value)
    {
        _recycle_show();
    }

    protected void _recycle_show()
    {
        RectTransform viewport = scrollRect.viewport;
        RectTransform content = scrollRect.content;
        // //把需要显示的物体显示
        var realSize = GetChildRealSize();

        //实际偏移
        var offset = content.anchoredPosition;
        //可显示区域 (rect 是从左下角开始的)
        Rect showRect = new Rect(maskRect.position - offset - new Vector2(0, maskRect.size.y - realSize.y),
            maskRect.size);

        needRecycle.Clear();
        //不需要显示的物体回收
        foreach (var item in showingItemDic)
        {
            var scrollListViewItem = item.Value;
            Rect itemRect = scrollListViewItem.rectTransform.rect;
            var overlaps = showRect.Overlaps(itemRect);
            if (overlaps)
            {
                continue;
            }

            needRecycle.Add(item.Key);
        }

        foreach (var index in needRecycle)
        {
            var viewItem = showingItemDic[index];
            showingItemDic.Remove(index);
            RecycleItem(viewItem);
        }

        //获取显示区域的左上角和右下角,来规范需要显示的范围(这里rect假设的是pos为左上角)
        Vector2 minPos = showRect.position - realSize;
        Vector2 maxPos = showRect.position + showRect.size;
        //最小最大显示行列 (向上取整) 其实可以通过这种方式计算显影.但是.不优雅(虽然效率很高)
        var minRowCol = minPos / realSize;
        var maxRowCol = maxPos / realSize;

        // 使用缓存的计算方式
        foreach (var cache in itemRectDic)
        {
            var index = cache.Key;
            var (row, col, rect) = cache.Value;
            if (!showRect.Overlaps(rect))
            {
                continue;
            }

            //显示中
            if (showingItemDic.ContainsKey(index))
            {
                continue;
            }

            var viewItem = this.GetFreeItem();
            showingItemDic.Add(index, viewItem);
            var data = this.adapter.GetData(index);
            viewItem.SetData(data);
            viewItem.rectTransform.anchoredPosition = rect.position;
        }
    }

    /// <summary>
    /// 子物体占用实际大小
    /// </summary>
    /// <returns></returns>
    protected Vector2 GetChildRealSize()
    {
        var realSize = childSize + new Vector2(viewParams.spaceing, viewParams.spaceing);
        return realSize;
    }

    /// <summary>
    /// 暂时不兼容改变大小的情况
    /// TODO 完善这个工具
    /// </summary>
    /// <returns></returns>
    protected Rect ConvertRectTransformPositionToTarget(RectTransform source, RectTransform target, Camera camera)
    {
        var componentInParent = rectTransform.GetComponentInParent<Canvas>();
        var worldCamera = componentInParent.worldCamera;
        Vector3[] corners = new Vector3[4];
        source.GetWorldCorners(corners); // 获取image的世界坐标四个顶点
        Vector2[] corner2s = new Vector2[4];
        // 将image的世界坐标转换为目标层级下的局部坐标
        for (int i = 0; i < 4; i++)
        {
            corners[i] = RectTransformUtility.WorldToScreenPoint(worldCamera, corners[i]);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(target, corners[i], worldCamera, out corner2s[i]);
        }

        // 计算image在目标层级下的位置和大小
        // TODO 这里需要根据是配方式进行修改实际数值.但是.无所谓,这里就这样的
        Rect rect = new Rect(corner2s[1], corner2s[2] - corner2s[0]);
        // 计算image在目标层级下的位置和大小
        // Rect rect = new Rect(Vector2.zero, source.rect.size);
        return rect;
    }

    /// <summary>
    /// 刷新
    /// </summary>
    public void RefreshAll(bool resetPos = false)
    {
        var scrollRectContent = scrollRect.content;

        //刷新content大小
        var dataCount = this.adapter.GetDataCount();

        //一个组件占用的实际大小
        var realSize = GetChildRealSize();

        var count = (int)Math.Ceiling((dataCount * 1f) / viewParams.num);

        //确定 行/列 (横向和竖向的派别本质上就是颠倒行列)
        var (row, column) = viewParams.dir == ScrollListViewDir.horizaontal
            ? (viewParams.num, count)
            : (count, viewParams.num);

        // content 实际大小
        var contentSize = new Vector2(column * realSize.x, row * realSize.y);
        scrollRectContent.sizeDelta = contentSize;

        //TODO  重新计算位置.但是现在懒得算了

        if (resetPos)
        {
            scrollRectContent.anchoredPosition = Vector2.zero;
        }

        //重新计算出子物体rect缓存
        itemRectDic.Clear();

        //根据排列来计算和填充
        for (int index = 0; index < dataCount; index++)
        {
            int realRow, realCol = 0;

            //横向排列先填充列
            if (viewParams.dir == ScrollListViewDir.horizaontal)
            {
                realRow = index % row;
                realCol = index / row;
            }
            //竖向排列先填充行
            else
            {
                realCol = index % column;
                realRow = index / column;
            }

            Rect rect = new Rect(new Vector2(realSize.x * realCol, -realSize.y * realRow), realSize);
            itemRectDic.Add(index, (realCol, realRow, rect));
        }

        //显示区域回收判定
        _recycle_show();
        //刷新显示当前显示数据(一定会有重复刷新的问题)
        foreach (var scrollListViewItem in this.showingItemDic)
        {
            int index = scrollListViewItem.Key;
            var viewItem = scrollListViewItem.Value;
            var data = this.adapter.GetData(index);
            viewItem.SetData(data);
        }
    }

    /// <summary>
    /// 获取一个新的Item
    /// </summary>
    /// <returns></returns>
    protected ScrollListViewItem GetFreeItem()
    {
        ScrollListViewItem item = null;

        if (poolItems.Count > 0)
        {
            item = poolItems.Pop();
        }
        else
        {
            item = this.adapter.GetItem();
        }

        item.rectTransform.SetParent(scrollRect.content);
        SetRectTransformNormal(item.rectTransform);
        item.SetActive(true);
        return item;
    }

    /// <summary>
    /// 回收
    /// </summary>
    /// <param name="item"></param>
    protected void RecycleItem(ScrollListViewItem item)
    {
        //直接放这里也行
        item.SetActive(false);
        poolItems.Add(item);
    }

    public override void OnDestroy()
    {
    }
}

/// <summary>
/// 循环列表子组件
/// </summary>
public abstract class ScrollListViewItem : UINodeBase
{
    protected ScrollListViewData data;

    public virtual void SetData(ScrollListViewData data)
    {
        this.data = data;
    }
}

public interface ScrollListViewData
{
}

/// <summary>
///  滚动列表适配器
/// </summary>
public interface ScrollListViewAdapter
{
    /// <summary>
    /// 获取一个新的物体实例
    /// </summary>
    public ScrollListViewItem GetItem();

    /// <summary>
    /// 获取数据
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public ScrollListViewData GetData(int index);

    /// <summary>
    /// 获取数据个数
    /// </summary>
    public int GetDataCount();
}