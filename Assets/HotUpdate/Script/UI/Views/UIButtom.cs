using UnityEngine.UI;

[UIBase(uiName: "UI_BUTTOM", uiPath: "Assets/GameMain/Prefabs/UI/HotKeyColumn.prefab", layer: 100)]
public class UIButtom : UIBase
{
    protected Button bag_btn;
    protected Button shop_btn;

    public override void OnInit()
    {
        bag_btn = this.transform.Find("content/bag_btn").GetComponent<Button>();
        bag_btn.onClick.AddListener(() => { UIManager.Inst().OpenUI("UI_BAG"); });

        shop_btn = this.transform.Find("content/shop_btn").GetComponent<Button>();
        shop_btn.onClick.AddListener(() => { UIManager.Inst().OpenUI("UI_SHOP"); });
    }

    public override void OnDestroy()
    {
    }
}