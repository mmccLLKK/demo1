using UnityEngine.UI;

[UIBase(uiName: "UI_BUTTOM", uiPath: "Assets/GameMain/Prefabs/UI/HotKeyColumn.prefab", layer: 100)]
public class UIButtom : UIBase
{
    protected Button bag_btn;

    public override void OnInit()
    {
        bag_btn = this.transform.Find("content/bag_btn").GetComponent<Button>();
        bag_btn.onClick.AddListener(() => { UIManager.Inst().OpenUI("UI_BAG"); });
    }

    public override void OnClose()
    {
    }
}