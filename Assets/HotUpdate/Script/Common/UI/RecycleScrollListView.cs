using UnityEngine;
using UnityEngine.UI;

public class ListData : ScrollListViewData
{
    public int index;
}

public class ScrollListViewItemTemp : ScrollListViewItem
{
    protected Text text;

    public override void OnInit()
    {
        text = this.gameObject.GetComponentInChildren<Text>(true);
    }

    // public void SetData(ListData t)
    // {
    //     base.SetData(t);
    //     if (_textMesh)
    //     {
    //         _textMesh.text = t.index.ToString();
    //     }
    // }
    public override void SetData(ScrollListViewData data)
    {
        base.SetData(data);
        var listData = data as ListData;
        if (text && listData != null)
        {
            text.text = listData.index.ToString();
        }
    }

    public override void OnDestroy()
    {
    }
}

[RequireComponent(typeof(ScrollRect))]
public class RecycleScrollListView : MonoBehaviour, ScrollListViewAdapter
{
    protected ScrollListView scrollListView;

    public Image image;

    [Tooltip("列表参数")] public ScrollListViewParams viewParams;

    public void Start()
    {
        image.gameObject.SetActive(false);

        scrollListView = new ScrollListView();
        scrollListView.Init(gameObject);

        scrollListView.SetRectTransformNormal(image.rectTransform);

        scrollListView.SetAdapter(this);
        scrollListView.SetParams(new ScrollListViewParams()
        {
            dir = ScrollListViewDir.vertical,
            num = 3,
            spaceing = 10f,
        });
        scrollListView.RefreshAll(true);
    }

    public void Update()
    {
        if (!image || scrollListView == null)
        {
            return;
        }

        // var position = scrollListView.scrollRect.content.anchoredPosition;
        // var maskRect = scrollListView.maskRect;
        //
        // Debug.Log($"position {position} maskRect {maskRect}");
        // Rect rect = new Rect(maskRect.position - position, maskRect.size);
        //
        // image.rectTransform.anchoredPosition = rect.position;
        // image.rectTransform.sizeDelta = new Vector2(rect.width, rect.height);
    }

    public ScrollListViewItem GetItem()
    {
        var instantiate = Instantiate(image.gameObject);
        var scrollListViewItem = new ScrollListViewItemTemp();
        scrollListViewItem.Init(instantiate);
        return scrollListViewItem;
    }

    public ScrollListViewData GetData(int index)
    {
        return new ListData()
        {
            index = index,
        };
    }

    public int GetDataCount()
    {
        return 100;
    }
}