using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Script.Battle.Ability.Axceler
{
    /// <summary>
    /// 当前技能配置
    /// </summary>
    [AbilityConfig("axceler_normal_attack")]
    public class Ability_Normal_Attack_Config : AbilityConfigBase
    {
        public string test;
    }

    /// <summary>
    /// 暂时没有技能编辑器时候的特殊实现
    /// 这里先直接写死
    /// </summary>
    [AbilityType("axceler_normal_attack")]
    public class Ability_Normal_Attack : AbilityMultipleCastBase
    {
        protected int stepMax = 4;

        protected int stepCur = 1;

        protected List<Task> tasks = new();
        
        /// <summary>
        /// 这里相当于写死了. 因为一般情况下 技能每一段的长度都是由 Timeline 决定的
        /// </summary>
        protected Dictionary<int, (string, float)> stepInfo = new()
        {
            {1, ("AttackA", 0.7f)},
            {2, ("AttackB", 0.7f)},
            {3, ("AttackC", 1f)},
            {4, ("AttackD", 1f)},
        };

        public override void CasteStart()
        {
            var task = NextStep();
        }

        protected async Task NextStep()
        {
            abilityStatus = AbilityStatus.CantCast;
            var (animName, time) = stepInfo[stepCur];
            Debug.Log($"当前使用到技能 Ability_Normal_Attack 的第 {stepCur} 段");
            owner.playActoin(animName);

            owner.canMove = false;
            owner.canRotate = false;

            //这里接受的是毫秒的int
            await Task.Delay((int) (time * 1000));

            owner.canMove = true;
            owner.canRotate = true;

            stepCur++;
            if (stepCur > stepMax)
            {
                //技能释放结束
                this.OnAbilityEnd?.Invoke();

                //简略的清空
                this.stepCur = 1;
            }

            abilityStatus = AbilityStatus.CanCast;
        }
    }
}