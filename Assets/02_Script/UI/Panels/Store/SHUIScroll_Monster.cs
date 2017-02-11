using UnityEngine;
using System;
using System.Collections;

using ListMonster = System.Collections.Generic.List<SHUIScrollSlot_Monster>;

public class SHUIScroll_Monster : SHUIMassiveScrollView
{
    #region Members : Inspector
    #endregion


    #region Members : Info
    private ListMonster m_pMonster = new ListMonster();
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
        var pMonster = pObject.GetComponent<SHUIScrollSlot_Monster>();
        pMonster.Initialize((eMonsterType)iType, (eMonsterType)(iType + 1));

        if (false == m_pMonster.Contains(pMonster))
            m_pMonster.Add(pMonster);
    }
    #endregion


    #region Interface Functions
    public void Initialize()
    {
        SetCellCount(GetMaxMonster() / 2);
    }
    #endregion


    #region Utility Functions
    int GetMaxMonster()
    {
        return (int)(eMonsterType.Max_NormalMonster - 1);
    }
    void RefleshSlotForSelect()
    {
        SHUtils.ForToList(m_pMonster, (pSlot) =>
        {
            pSlot.SetGoodsState();
        });
    }
    #endregion


    #region Event Handler
    public void OnClickToSlot(SHUIWidget_Monster pMonster)
    {
        if (null == pMonster)
            return;

        var eUseType = Single.Inventory.GetMonsterGoodsStateToPlayerPrefs(pMonster.m_eType);
        switch(eUseType)
        {
            case eGoodsState.NotHas:
                if (Single.Inventory.m_iCoin < pMonster.GetPrice())
                {
                    Single.UI.ShowNotice("알림",
                        string.Format("{0} 코인이 부족합니다.", pMonster.GetPrice() - Single.Inventory.m_iCoin));
                }
                else
                {
                    Single.UI.ShowNotice_TwoBtn("알림",
                        string.Format("{0} 코인을 소모합니다.\n구매 하시겠습니까?", pMonster.GetPrice()),
                        () =>
                        {
                            Single.Inventory.ConsumeCoin(pMonster.GetPrice());
                            Single.Inventory.SetMonsterTypeToPlayerPrefs(pMonster.m_eType, eGoodsState.Enable);
                        }, null);
                }
                break;
            case eGoodsState.Disable:
                Single.Inventory.SetMonsterTypeToPlayerPrefs(pMonster.m_eType, eGoodsState.Enable);
                break;
            case eGoodsState.Enable:
                Single.Inventory.SetMonsterTypeToPlayerPrefs(pMonster.m_eType, eGoodsState.Disable);
                break;
        }
        RefleshSlotForSelect();
    }
    #endregion
}
