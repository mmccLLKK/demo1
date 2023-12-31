using Newtonsoft.Json;

namespace Script.StaticConfig
{
    public class StaticConfig
    {
        /// <summary>
        /// 暂时没有导表或者配置系统.使用代码进行静态配置
        /// </summary>
        public static void InitConfig()
        {
            var confManager = ConfManager.inst;
            var roleConfigsTables = confManager.roleConfigs.tables;
            var attrConfigsTables = confManager.attrConfigs.tables;
            var roleTemplateConfigs = confManager.roleTemplateConfigs.tables;
            var abilityConfigsTables = confManager.abilityConfigs.tables;
            //大剑的骑士
            roleConfigsTables.Add(new RoleConfig()
                { id = "GreatSword", roleAttrId = "GreatSword", roleTemplateId = "GreatSword" });
            attrConfigsTables.Add(new RoleAttrConfig() { id = "GreatSword", level = 1, hp = 100, atk = 10 });
            roleTemplateConfigs.Add(new RoleTemplateConfig()
                { id = "GreatSword", name = "大剑", path = "Assets/GameMain/Prefabs/Characters/Role1/Role1.prefab" });
            //TODO 技能数据的配置
            abilityConfigsTables.Add(new AbilityConfig
            {
                id = "GreatSword_normal_attack_1",
                templateName = "normal_attack",
                confStr = JsonConvert.SerializeObject(new AbilityNormalAttackConfig
                {
                    duringTime = 1f,
                    exitTime = 0.8f,
                    animName = "attack_1",
                    moveSpeed = 0.2f,
                })
            });
            //关卡信息
            var battleLevelConfigs = confManager.battleLevelConfigs.tables;
            battleLevelConfigs.Add(new BattleLevelConfig
            {
                id = "level_demo_1",
                path = "Assets/GameMain/Scene/newCharacter.unity"
            });
            battleLevelConfigs.Add(new BattleLevelConfig
            {
                id = "level_demo_2",
                path = "Assets/GameMain/Scene/battle1.unity"
            });
        }
    }
}