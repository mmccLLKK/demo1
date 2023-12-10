using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

[AttributeUsage(AttributeTargets.Class)]
public class AbilityTypeAttribute : Attribute
{
    public string templateName { get; set; }

    public AbilityTypeAttribute(string templateName)
    {
        this.templateName = templateName;
    }
}

[AttributeUsage(AttributeTargets.Class)]
public class AbilityConfigAttribute : Attribute
{
    public string templateName { get; set; }

    public AbilityConfigAttribute(string templateName)
    {
        this.templateName = templateName;
    }
}

public struct AbilityInfo
{
    public string templateName;
    public Type type;
    public Type confType;
}

public class AbilityFactory
{
    protected static bool isInit = false;

    protected static Dictionary<string, AbilityInfo> abilityInfos = new();

    public static void Init()
    {
        if (isInit)
        {
            return;
        }

        Dictionary<string, Type> typeDic = new();
        Dictionary<string, Type> configTypeDic = new();

        isInit = true;
        Assembly assembly = Assembly.GetExecutingAssembly();
        Type[] types = assembly.GetTypes();
        object[] attributes = null;
        foreach (Type type in types)
        {
            // 能力类型
            attributes = type.GetCustomAttributes(typeof(AbilityTypeAttribute), true);
            if (attributes.Length > 0)
            {
                AbilityTypeAttribute abilityTypeAttribute = (AbilityTypeAttribute) attributes[0];
                var templateName = abilityTypeAttribute.templateName;
                typeDic.Add(templateName, type);
                continue;
            }

            // 能力配置类型
            attributes = type.GetCustomAttributes(typeof(AbilityConfigAttribute), true);
            if (attributes.Length > 0)
            {
                AbilityConfigAttribute abilityConfigAttribute = (AbilityConfigAttribute) attributes[0];
                var templateName = abilityConfigAttribute.templateName;
                configTypeDic.Add(templateName, type);
                continue;
            }
        }

        //填充
        foreach (var keyValuePair in typeDic)
        {
            var templateName = keyValuePair.Key;
            var type = keyValuePair.Value;
            var confType = configTypeDic.GetValueOrDefault(templateName, null);

            abilityInfos.Add(templateName, new AbilityInfo
            {
                templateName = templateName,
                type = type,
                confType = confType,
            });
        }
    }

    /// <summary>
    /// 创建能力
    /// </summary>
    /// <param name="templateName">模板</param>
    /// <param name="configStr">配置</param>
    public static AbilityBase CreateAbility(string templateName, string configStr)
    {
        //尝试初始化
        Init();

        // 模板信息
        if (!abilityInfos.ContainsKey(templateName))
        {
            throw new Exception($"技能模板不存在 {templateName}");
        }

        var info = abilityInfos[templateName];
        var abilityType = info.type;
        var infoConfType = info.confType;

        AbilityConfigBase config = null;
        if (infoConfType != null)
        {
            config = JsonConvert.DeserializeObject(configStr, infoConfType) as AbilityConfigBase;
        }

        var instance = Activator.CreateInstance(abilityType) as AbilityBase;
        if (instance == null)
        {
            throw new Exception($"技能初始化失败 {templateName}");
        }

        instance.Init(config);
        return instance;
    }
}