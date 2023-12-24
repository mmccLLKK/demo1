using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopScrollData : ScrollListViewData
{
    public bool is_select;

    public ShopItemData data;
}

public class ShopScrollViewItem : ScrollListViewItem
{
    public UIShopItemView shopItemView;
    public Action<int> onClick;
    protected ShopScrollData _data;

    public override void OnInit()
    {
        shopItemView = this.gameObject.GetComponent<UIShopItemView>();
        shopItemView.Button.onClick.AddListener(() =>
        {
            var id = this._data.data.id;
            this.onClick?.Invoke(id);
            Debug.Log($"点击了 ShopScrollViewItem {id}");
        });
    }

    public override void SetData(ScrollListViewData data)
    {
        base.SetData(data);
        _data = data as ShopScrollData;
        var shopItemData = _data.data;
        shopItemView.select.SetActive(_data.is_select);
        shopItemView.textName.text = shopItemData.name;
        shopItemView.textPrice.text = $"物品价格:{shopItemData.price}";
        UIManager.Inst().SetSprite(UIData.UI_ATLAS_ITEM_ICON, shopItemData.icon, shopItemView.icon);
    }

    public override void OnDestroy()
    {
    }
}


public class UIShopView : MonoBehaviour, ScrollListViewAdapter
{
    public Button btnClose;

    public Button btnBuy;

    public GameObject shopItem;

    public ScrollRect shopScroll;

    protected ScrollListView scrollListView;

    /// <summary>
    /// 选中一个物品
    /// </summary>
    public Action<int> onSelectItem;

    /// <summary>
    /// 点击关闭按钮
    /// </summary>
    public Action onClickClose;

    protected List<ShopScrollData> _datas = new();

    public void Init()
    {
        this.btnClose.onClick.AddListener(() => { this.onClickClose?.Invoke(); });

        scrollListView = new ScrollListView();
        scrollListView.Init(shopScroll.gameObject);

        scrollListView.SetAdapter(this);
        scrollListView.SetParams(new ScrollListViewParams()
        {
            dir = ScrollListViewDir.vertical,
            num = 2,
            spaceing = 10f,
        });
        scrollListView.RefreshAll(true);
    }

    public void SetData(List<ShopItemData> datas)
    {
        for (var i = 0; i < datas.Count; i++)
        {
            var shopItemData = datas[i];
            var shopScrollData = new ShopScrollData()
            {
                data = shopItemData,
            };
            this._datas.Add(shopScrollData);
        }

        this.scrollListView.RefreshAll(true);
    }

    public void SetSelect(int id)
    {
        foreach (var shopScrollData in this._datas)
        {
            shopScrollData.is_select = shopScrollData.data.id == id;
        }

        this.scrollListView.RefreshAll();
    }


    public ScrollListViewItem GetItem()
    {
        var instantiate = Instantiate(shopItem);
        var item = new ShopScrollViewItem();
        item.Init(instantiate);
        item.onClick = (id) => { onSelectItem?.Invoke(id); };
        return item;
    }

    public ScrollListViewData GetData(int index)
    {
        return this._datas[index];
    }

    public int GetDataCount()
    {
        return this._datas.Count;
    }
}