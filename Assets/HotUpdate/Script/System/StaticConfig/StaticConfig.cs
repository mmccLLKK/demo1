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
            //拿斧头的小萝莉初始化 (普通)
            roleConfigsTables.Add(new RoleConfig() {id = "axceler", roleAttrId = "axceler", roleTemplateId = "axceler"});
            attrConfigsTables.Add(new RoleAttrConfig() {id = "axceler", level = 1, hp = 100, atk = 10});
            roleTemplateConfigs.Add(new RoleTemplateConfig() {id = "axceler", name = "斧头小萝莉", path = "Assets/GameMain/Prefabs/Characters/Role3/Role3.prefab"});
            //TODO 技能数据的配置
            abilityConfigsTables.Add(new AbilityConfig
            {
                id = "axceler_normal_attack",
                templateName = "axceler_normal_attack",
                confStr = "{}" // TODO  后续使用编辑器接入真配置, 这里目前使用的假配置
            });
            //拿剑的骑士

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