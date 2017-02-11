using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using ListStick = System.Collections.Generic.List<SHUIScrollSlot_Stick>;

public class SHUIScroll_Stick : SHUIMassiveScrollView
{
    #region Members : Inspector
    #endregion


    #region Members : Info
    private ListStick m_pSticks = new ListStick();
    #endregion


    #region System Functions
    #endregion


    #region Virtual Functions
    protected override void OnInitialized()
    {
        Initialize();
    }
    protected override void SetSlotData(GameObject pObject, int iIndex)
    {
        int iType = (iIndex * 2) + 1;
        var pSlot = pObject.GetComponent<SHUIScrollSlot_Stick>();
        pSlot.Initialize((eStickType)iType, (eStickType)(iType + 1));

        if (false == m_pSticks.Contains(pSlot))
            m_pSticks.Add(pSlot);
    }
    #endregion


    #region Interface Functions
    public void Initialize()
    {
        SetCellCount(GetMaxStick() / 2);
    }
    #endregion


    #region Utility Functions
    int GetMaxStick()
    {
        return (int)(eStickType.Max - 1);
    }
    void RefleshSlotForSelect()
    {
        SHUtils.ForToList(m_pSticks, (pSlot) =>
        {
            pSlot.SetSelector();
        });
    }
    #endregion


    #region Event Handler
    public void OnClickToSlot(SHUIWidget_Stick pStick)
    {
        if (null == pStick)
            return;

        var eUseType = Single.Inventory.GetStickGoodsStateToPlayerPrefs(pStick.m_eType);
        switch(eUseType)
        {
            case eGoodsState.NotHas:
                if (Single.Inventory.m_iCoin < pStick.GetPrice())
                {
                    Single.UI.ShowNotice(Localization.Get("POPUP_TITLE_NOTICE"),
                        Localization.Format("STORE_COIN_SHORTAGE", (pStick.GetPrice() - Single.Inventory.m_iCoin)));
                }
                else
                {
                    Single.UI.ShowNotice_TwoBtn("알림",
                        string.Format("{0} 코인을 소모합니다.\n구매 하시겠습니까?", pStick.GetPrice()),
                        () => 
                        {
                            Single.Inventory.ConsumeCoin(pStick.GetPrice());
                            Single.Inventory.SetStickTypeToPlayerPrefs(pStick.m_eType, eGoodsState.Enable);
                        }, null);
                }
                break;
            case eGoodsState.Disable:
                Single.Inventory.SetStickTypeToPlayerPrefs(pStick.m_eType, eGoodsState.Enable);
                break;
            case eGoodsState.Enable:
                Single.Inventory.SetStickTypeToPlayerPrefs(pStick.m_eType, eGoodsState.Disable);
                break;
        }
        RefleshSlotForSelect();
    }
    #endregion
}