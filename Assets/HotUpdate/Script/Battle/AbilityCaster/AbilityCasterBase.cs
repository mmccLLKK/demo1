/// <summary>
/// 优先满足一般需求
/// </summary>
public class AbilityCasterBase
{
    protected AbilityBase abilityBase;

    public void SetAbility(AbilityBase ability)
    {
        this.abilityBase = ability;
    }
}