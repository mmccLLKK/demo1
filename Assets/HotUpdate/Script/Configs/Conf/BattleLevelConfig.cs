public class BattleLevelConfig
{
    public string id;

    public string path;
}

public class BattleLevelConfigs : ConfigsBase<BattleLevelConfig>
{
    public BattleLevelConfig GetLevelById(string id)
    {
        var battleLevelConfig = this.tables.Find(table => table.id.Equals(id));
        return battleLevelConfig;
    }
}