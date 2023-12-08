using UnityEngine.UI;

public class ItemInfo
{
    public int id;
    public int num;
    public string icon;
    public string name;
}

[UIBase(uiName: "UI_BAG", uiPath: "Assets/GameMain/Prefabs/UI/Bag/Bag.prefab", layer: 200)]
public class UIBag : UIBase
{
    protected Button closeBtn;

    public override void OnInit()
    {
        closeBtn = this.transform.Find("content/CloseBtn").GetComponent<Button>();
        closeBtn.onClick.AddListener(() => { CloseUI(); });

        // 设置图片的演示
        var image = this.transform.Find("Bag_item/content/icon").GetComponent<Image>();
        UIManager.Inst().SetSprite(UIData.UI_ATLAS_ITEM_ICON, "21", image);
    }

    public override void OnDestroy()
    {
    }
}