using System.Collections.Generic;

public class AbilityConfig
{
    public string id;
    public string templateName;
    public string confStr;
}

public class AbilityConfigs : ConfigsBase<AbilityConfig>
{
    /// <summary>
    /// 获取指定的技能
    /// </summary>
    public AbilityConfig GetAbnConfigs(string abilityId)
    {
        var abilityConfig = tables.Find(table => table.id.Equals(abilityId));
        return abilityConfig;
    }
}