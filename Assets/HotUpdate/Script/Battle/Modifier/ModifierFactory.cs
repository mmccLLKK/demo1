using System;
using System.Collections.Generic;
using Newtonsoft.Json;

/// <summary>
/// 效果器基础配置
/// </summary>
public class ModifierConfigBase
{
}


/// <summary>
/// 效果器配置的注解
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class ModifierConfigAttribute : CommonAttribute
{
    public ModifierConfigAttribute(string templateName) : base(templateName)
    {
    }
}

/// <summary>
/// 效果器类的注解
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class ModifierTypeAttribute : CommonAttribute
{
    public ModifierTypeAttribute(string templateName) : base(templateName)
    {
    }
}

/// <summary>
/// 效果类定义
/// </summary>
public struct ModifierDefInfo
{
    public string template;

    public Type type;

    public Type configType;
}

/// <summary>
/// 效果器工厂
/// </summary>
public class ModifierFactory
{
    protected static bool _isInit = false;

    protected static Dictionary<string, ModifierDefInfo> defInfos = new();

    protected static void Init()
    
    {
        if (_isInit)
        {
            return;
        }

        _isInit = true;

        var modifierTypes = CommonAttributeUtils.GetAllBeAttributeType<ModifierTypeAttribute>();
        var modifierConfigs = CommonAttributeUtils.GetAllBeAttributeType<ModifierConfigAttribute>();

        foreach (var keyValuePair in modifierTypes)
        {
            var key = keyValuePair.Key;
            var type = keyValuePair.Value;
            var configType = modifierConfigs.GetValueOrDefault(key, typeof(ModifierConfigBase));

            defInfos.Add(key, new ModifierDefInfo()
            {
                template = key,
                type = type,
                configType = configType,
            });
        }
    }

    /// <summary>
    /// 创建一个效果器
    /// </summary>
    public static ModifierBase Create(string templateName, string configStr)
    {
        Init();

        if (!defInfos.ContainsKey(templateName))
        {
            throw new Exception($"未找到效果器 {templateName}");
        }

        var modifierDefInfo = defInfos[templateName];

        var config = JsonConvert.DeserializeObject(configStr, modifierDefInfo.configType) as ModifierConfigBase;

        var instance = Activator.CreateInstance(modifierDefInfo.type) as ModifierBase;

        instance.SetConfig(config);

        return instance;
    }
}