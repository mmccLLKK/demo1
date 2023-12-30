using System;
using System.Collections.Generic;
using System.Reflection;

public abstract class CommonAttribute : Attribute
{
    public string templateName { get; set; }

    public CommonAttribute(string templateName)
    {
        this.templateName = templateName;
    }
}

/// <summary>
/// 注解工具
/// </summary>
public class CommonAttributeUtils
{
    /// <summary>
    /// 获取所有被指定注解标记的类
    /// </summary>
    public static Dictionary<string, Type> GetAllBeAttributeType<T>() where T : CommonAttribute
    {
        Dictionary<string, Type> rst = new();

        Assembly assembly = Assembly.GetExecutingAssembly();
        Type[] types = assembly.GetTypes();
        object[] attributes = null;
        foreach (Type type in types)
        {
            // 能力类型
            attributes = type.GetCustomAttributes(typeof(T), true);
            if (attributes.Length == 0)
            {
                continue;
            }

            T attribute = (T)attributes[0];
            var templateName = attribute.templateName;
            rst.Add(templateName, type);
        }

        return rst;
    }
}