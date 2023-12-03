using Cysharp.Threading.Tasks;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    protected GameObject root;

    protected UIMap _uiMap;
    protected UIButtom _buttom;

    private void Start()
    {
        InitUI();
    }

    protected async UniTask InitUI()
    {
        _uiMap = await UIManager.Inst().OpenUI("UI_MAP") as UIMap;
        _buttom = await UIManager.Inst().OpenUI("UI_BUTTOM") as UIButtom;
    }

    private void OnDestroy()
    {
        _uiMap?.CloseUI();
        _buttom?.CloseUI();
    }
}