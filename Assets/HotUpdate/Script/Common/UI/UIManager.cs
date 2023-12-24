using System;
using System.Collections.Generic;
using System.Reflection;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.U2D;
using UnityEngine.UI;
using Object = UnityEngine.Object;

// 自定义修饰器
[AttributeUsage(AttributeTargets.Class)]
public class UIBaseAttribute : Attribute
{
    /// <summary>
    /// UI名.用于打开UI
    /// </summary>
    public string uiName { get; set; }

    /// <summary>
    /// UI资源路径
    /// </summary>
    public string uiPath { get; set; }

    /// <summary>
    /// UI层级
    /// </summary>
    public int layer { get; set; }


    public UIBaseAttribute(string uiName, string uiPath, int layer)
    {
        this.uiName = uiName;
        this.uiPath = uiPath;
        this.layer = layer;
    }
}

public struct UIInfo
{
    public Type type;
    public string path;
    public int layer;
}

public class UIManager : MonoBehaviour
{
    protected Dictionary<string, UIInfo> uiDataMapping = new Dictionary<string, UIInfo>();
    protected Dictionary<string, UIBase> uiDict = new Dictionary<string, UIBase>();
    protected Dictionary<string, SpriteAtlas> uiSpriteAtlasDic = new Dictionary<string, SpriteAtlas>();

    protected static UIManager _inst;

    public static UIManager Inst()
    {
        return _inst;
    }

    /// <summary>
    /// ui挂载点
    /// </summary>
    public Transform uiAnchor;

    public void Init()
    {
        if (_inst)
        {
            return;
        }

        _inst = this;

        DontDestroyOnLoad(this);

        Assembly assembly = Assembly.GetExecutingAssembly();
        Type[] types = assembly.GetTypes();

        foreach (Type type in types)
        {
            object[] attributes = type.GetCustomAttributes(typeof(UIBaseAttribute), true);
            if (attributes.Length > 0)
            {
                UIBaseAttribute uiBaseAttribute = (UIBaseAttribute)attributes[0];
                var uiName = uiBaseAttribute.uiName;
                Debug.Log($"注册UI实例: Class Name: {type.Name}, uiName: {uiName} 层级: {uiBaseAttribute.layer}");
                var uiInfo = new UIInfo() { type = type, path = uiBaseAttribute.uiPath, layer = uiBaseAttribute.layer };
                uiDataMapping.Add(uiName, uiInfo);
            }
        }
    }


    /// <summary>
    /// 打开UI
    /// </summary>
    /// <param name="uiName">需要传入UI名</param>
    public async UniTask<UIBase> OpenUI(string uiName)
    {
        Debug.Log($"打开UI {uiName}");

        //初始化类
        if (!uiDataMapping.ContainsKey(uiName))
        {
            throw new Exception($"未注册的UI {uiName}");
        }

        //这里的逻辑可以视情况而定,本质上是不允许打开同样的UI.直接报错也可以,应该成为项目中大家都认可的规范
        if (uiDict.ContainsKey(uiName))
        {
            return uiDict[uiName];
        }

        UIBase ui = null;
        try
        {
            var uiInfo = uiDataMapping[uiName];

            //加载ui资源
            var asset = await Addressables.LoadAssetAsync<Object>(uiInfo.path);
            var go = GameObject.Instantiate(asset) as GameObject;

            Canvas canvas = go.GetComponent<Canvas>();
            //添加修改
            if (!canvas)
            {
                canvas = go.AddComponent<Canvas>();
                go.AddComponent<GraphicRaycaster>();
            }

            //挂载UI到面板
            go.transform.SetParent(this.uiAnchor, false);

            //设置层级
            canvas.overrideSorting = true;

            canvas.sortingOrder = uiInfo.layer;

            var rectTransform = go.GetComponent<RectTransform>();
            rectTransform.sizeDelta = Vector2.zero;

            //初始化UI脚本
            var instance = Activator.CreateInstance(uiInfo.type) as UIBase;
            instance.Init(go);
            instance.CloseUI = () => { this.CloseUI(uiName); };

            ui = instance;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            //报错了进行收尾
        }

        uiDict.Add(uiName, ui);

        Debug.Log($"打开UI {uiName} 完成");
        return ui;
    }

    public void CloseUI(string uiName)
    {
        Debug.Log($"关闭UI {uiName}");
        if (!uiDict.ContainsKey(uiName))
        {
            return;
        }

        // var uiInfo = uiDataMapping[uiName];
        var uiBase = uiDict[uiName];
        uiBase.OnDestroy();
        Destroy(uiBase.gameObject);
        // 释放资源
        // Addressables.Release(uiInfo.path);
        uiDict.Remove(uiName);
        Debug.Log($"关闭UI {uiName} 完成");
    }

    /// <summary>
    /// 设置图集中的图片(这里的使用比较粗暴)
    /// </summary>
    /// <param name="atlasName">图集名</param>
    /// <param name="icon">图标</param>
    public async UniTask SetSprite(string atlasName, string icon, Image image)
    {
        if (!uiSpriteAtlasDic.ContainsKey(atlasName))
        {
            SpriteAtlas spriteAtlas = await Addressables.LoadAssetAsync<SpriteAtlas>(UIData.UI_ATLAS_ITEM_ICON);
            uiSpriteAtlasDic.Add(atlasName, spriteAtlas);
        }

        var uiSpriteAtlas = uiSpriteAtlasDic[atlasName];

        image.sprite = uiSpriteAtlas.GetSprite(icon);
    }
}