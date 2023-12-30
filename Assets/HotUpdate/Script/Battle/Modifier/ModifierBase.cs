/// <summary>
/// 效果器
/// </summary>
public abstract class ModifierBase
{
    /// <summary>
    /// 效果器是需要明确知道
    /// </summary>
    public string templateName;

    protected ModifierConfigBase _config;

    public void SetConfig(ModifierConfigBase config)
    {
        this._config = config;
    }


    /// <summary>
    /// 
    /// </summary>
    public abstract void OnAttach();
}