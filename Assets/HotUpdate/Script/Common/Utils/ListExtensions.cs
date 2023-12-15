using System;
using System.Collections.Generic;

public static class ListExtensions
{
    // 扩展方法：移除并返回列表中的最后一个元素
    public static T Pop<T>(this List<T> list)
    {
        if (list.Count == 0)
        {
            throw new InvalidOperationException("列表为空");
        }

        int lastIndex = list.Count - 1;
        T lastItem = list[lastIndex];
        list.RemoveAt(lastIndex);

        return lastItem;
    }

    // 扩展方法：移除并返回列表中的第一个元素
    public static T Shift<T>(this List<T> list)
    {
        if (list.Count == 0)
        {
            throw new InvalidOperationException("列表为空");
        }

        T firstItem = list[0];
        list.RemoveAt(0);

        return firstItem;
    }
}