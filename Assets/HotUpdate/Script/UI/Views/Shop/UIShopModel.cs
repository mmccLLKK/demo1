using System.Collections.Generic;

public class ShopItemData
{
    public int id;
    public string name;
    public string icon;
    public int price;
}

/// <summary>
/// 商店UI数据层 (表现数据)
/// </summary>
public class UIShopModel
{
    protected List<ShopItemData> shopItemDatas = new();

    public void Init()
    {
        for (int i = 0; i < 100; i++)
        {
            var shopItemData = new ShopItemData()
            {
                id = i,
                name = $"商店物品{i}",
                price = ((int)(i + 100) / 10) * 10,
                icon = $"item_{i % 4 + 1}"
            };
            shopItemDatas.Add(shopItemData);
        }
    }

    public List<ShopItemData> GetAllItem()
    {
        return this.shopItemDatas;
    }
}