using UnityEngine;

[UIBase("UI_SHOP", "Assets/GameMain/Prefabs/UI/Shop/Shop.prefab", 100)]
public class UIShopController : UIBase
{
    protected UIShopView uiShopView;

    protected UIShopModel uiShopModel;

    public override void OnInit()
    {
        //初始化数据
        uiShopModel = new UIShopModel();
        uiShopModel.Init();
        //初始化表现
        uiShopView = this.gameObject.GetComponent<UIShopView>();
        uiShopView.Init();

        var shopItemDatas = uiShopModel.GetAllItem();
        uiShopView.SetData(shopItemDatas);

        uiShopView.onClickClose = () => { this.CloseUI(); };
        uiShopView.onSelectItem = (id) => { uiShopView.SetSelect(id); };
    }

    public override void OnDestroy()
    {
    }
}