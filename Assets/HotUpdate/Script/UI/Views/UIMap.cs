using UnityEngine;

[UIBase(uiName: "UI_MAP", uiPath: "Assets/GameMain/Prefabs/UI/MiniMap.prefab", layer: 100)]
public class UIMap : UIBase
{
    public override void OnInit()
    {
        Debug.Log("初始化地图");
    }

    public override void OnDestroy()
    {
        Debug.Log("地图UI销毁了");
    }
}