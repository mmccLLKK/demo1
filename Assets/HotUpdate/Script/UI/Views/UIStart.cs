﻿using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

[UIBase(uiName: "UI_START", uiPath: "Assets/GameMain/Prefabs/UI/Start.prefab", layer: 100)]
public class UIStart : UIBase
{
    public Button startBtn;

    public Button uiBtn;

    public override void OnInit()
    {
        var btn1 = this.transform.Find("Battle");
        startBtn = btn1.GetComponent<Button>();

        startBtn.onClick.AddListener(() =>
        {
            Addressables.LoadSceneAsync("Assets/GameMain/Scene/battle1.unity");
            CloseUI();
        });

        var btn2 = this.transform.Find("UI");
        uiBtn = btn2.GetComponent<Button>();
        uiBtn.onClick.AddListener(() =>
        {
            CloseUI();
            JumpToLevel("level_demo_2").GetAwaiter().GetResult();
        });

        var btn3 = this.transform.Find("Role");
        uiBtn = btn3.GetComponent<Button>();
        uiBtn.onClick.AddListener(() =>
        {
            // Addressables.LoadSceneAsync("Assets/GameMain/Scene/newCharacter.unity");
            CloseUI();
            JumpToLevel("level_demo_1").GetAwaiter().GetResult();
        });
    }

    public async UniTask JumpToLevel(string levelID)
    {
        var playerData = PlayerSystem.inst.curPlayerData;
        await BattleLevelManager.LoadLevel(levelID, playerData);
    }

    public override void OnDestroy()
    {
        Debug.Log("初始ui销毁");
    }
}