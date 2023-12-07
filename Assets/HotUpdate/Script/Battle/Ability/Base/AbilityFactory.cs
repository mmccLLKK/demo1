using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

[AttributeUsage(AttributeTargets.Class)]
public class AbilityTypeAttribute : Attribute
{
    public string abilityId { get; set; }

    public AbilityTypeAttribute(string id)
    {
        abilityId = id;
    }
}

[AttributeUsage(AttributeTargets.Class)]
public class AbilityConfigAttribute : Attribute
{
    public string abilityId { get; set; }

    public AbilityConfigAttribute(string id)
    {
        abilityId = id;
    }
}

public class AbilityFactory
{
    protected static bool isInit = false;

    protected static Dictionary<string, Type> abilityTypes = new();
    protected static Dictionary<string, Type> abilityConfigTypes = new();

    public void Init()
    {
        if (isInit)
        {
            return;
        }

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
                AbilityTypeAttribute uiBaseAttribute = (AbilityTypeAttribute) attributes[0];
                var abilityId = uiBaseAttribute.abilityId;
                continue;
            }

            // 能力配置类型
            attributes = type.GetCustomAttributes(typeof(AbilityTypeAttribute), true);
        }
    }

    /// <summary>
    /// 创建能力
    /// </summary>
    /// <param name="abilityId">模板</param>
    /// <param name="configStr">配置</param>
    public static AbilityBase CreateAbility(string abilityId, string configStr)
    {
        // 模板
        if (!abilityTypes.ContainsKey(abilityId))
        {
            throw new Exception($"技能模板不存在 {abilityId}");
        }

        // 模板配置
        if (!abilityConfigTypes.ContainsKey(abilityId))
        {
            throw new Exception($"技能模板配置不存在 {abilityId}");
        }

        var abilityType = abilityTypes[abilityId];
        var abilityConfigType = abilityConfigTypes[abilityId];
        var config = JsonConvert.DeserializeObject(configStr, abilityConfigType) as AbilityConfigBase;

        var instance = Activator.CreateInstance(abilityType) as AbilityBase;
        if (instance == null)
        {
            throw new Exception($"技能初始化失败 {abilityId}");
        }

        instance.Init(config);

        return instance;
    }
}