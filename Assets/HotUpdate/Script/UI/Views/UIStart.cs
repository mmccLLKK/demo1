using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

[UIBase(uiName: "UI_START", uiPath: "Assets/GameMain/Prefabs/UI/Start.prefab", layer: 100)]
public class UIStart : UIBase
{
    public override void OnInit()
    {
        Debug.Log("初始化 UIStart");

        var btn2 = this.transform.Find("Battle");
        var uiBtn = btn2.GetComponent<Button>();
        uiBtn.onClick.AddListener(() =>
        {
            CloseUI();
            JumpToLevel("level_demo_2");
        });


        var btn3 = this.transform.Find("Role");
        var roleBtn = btn3.GetComponent<Button>();
        roleBtn.onClick.AddListener(() =>
        {
            CloseUI();
            JumpToLevel("level_demo_1");
        });
    }

    protected async Task JumpToLevel(string levelID)
    {
        var playerData = PlayerSystem.inst.curPlayerData;
        await BattleLevelManager.LoadLevel(levelID, playerData);
    }

    public override void OnDestroy()
    {
        Debug.Log("初始ui销毁");
    }
}