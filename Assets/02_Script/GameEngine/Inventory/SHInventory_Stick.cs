using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using DicSticks = System.Collections.Generic.Dictionary<eStickType, eGoodsState>;

public enum eStickType
{
    None,
    Stick_1,
    Stick_2,
    Stick_3,
    Stick_4,
    Stick_5,
    Stick_6,
    Stick_7,
    Stick_8,
    Stick_9,
    Stick_10,
    Stick_11,
    Stick_12,
    Stick_13,
    Stick_14,
    Stick_15,
    Max,
}

public partial class SHInventory : SHBaseEngine
{
    #region Members
    private DicSticks m_dicStickInfo = new DicSticks();
    #endregion


    #region Interface : System
    public void InitStick()
    {
        RegisterBasicStick();
        ResetStickInfo();
    }
    public void OnUpdateStick()
    {

    }
    #endregion


    #region Interface : Dic Helpper
    public eStickType GetEnableSticksForDic()
    {
        foreach(var kvp in m_dicStickInfo)
        {
            if (eGoodsState.Enable != kvp.Value)
                continue;

            return kvp.Key;
        }

        return eStickType.None;
    }
    #endregion


    #region Interface : PlayerPrefs Helpper
    public void SetStickTypeToPlayerPrefs(eStickType eType, eGoodsState eGoods)
    {
        switch(eGoods)
        {
            case eGoodsState.Disable:
                if (1 >= GetEnableSticksToPlayerPrefs().Count)
                {
                    Single.UI.ShowNotice("알림", "꼬쟁이는 최소 1개 이상입니다.");
                    return;
                }
                break;
        }

        var pEnableSticks = GetEnableSticksToPlayerPrefs();
        SHPlayerPrefs.SetInt(string.Format("Inventory_Stick_{0}", (int)eType), (int)eGoods);
        switch (eGoods)
        {
            case eGoodsState.Enable:
                SHUtils.ForToList(pEnableSticks, (eStick) =>
                {
                    SetStickTypeToPlayerPrefs(eStick, eGoodsState.Disable);
                });
                break;
        }
    }
    public eGoodsState GetStickGoodsStateToPlayerPrefs(eStickType eType)
    {
        return (eGoodsState)SHPlayerPrefs.GetInt(
            string.Format("Inventory_Stick_{0}", (int)eType), (int)eGoodsState.NotHas);
    }
    public List<eStickType> GetEnableSticksToPlayerPrefs()
    {
        var pResult = new List<eStickType>();
        SHUtils.ForToEnum<eStickType>((eType) =>
        {
            if (false == IsEnableStickToPlayerPrefs(eType))
                return;

            pResult.Add(eType);
        });

        return pResult;
    }
    public bool IsHasStickToPlayerPrefs(eStickType eType)
    {
        return (eGoodsState.NotHas != GetStickGoodsStateToPlayerPrefs(eType));
    }
    public bool IsEnableStickToPlayerPrefs(eStickType eType)
    {
        return (eGoodsState.Enable == GetStickGoodsStateToPlayerPrefs(eType));
    }
    #endregion


    #region Utility Functions
    private void RegisterBasicStick()
    {
        RegisterStick(eStickType.Stick_1);
    }
    private void RegisterStick(eStickType eType)
    {
        if (true == IsHasStickToPlayerPrefs(eType))
            return;

        SetStickTypeToPlayerPrefs(eType, eGoodsState.Enable);
    }
    void ResetStickInfo()
    {
        m_dicStickInfo.Clear();
        SHUtils.ForToEnum<eStickType>((eType) =>
        {
            if ((eStickType.None == eType) ||
                (eStickType.Max  == eType))
                return;

            m_dicStickInfo.Add(eType, GetStickGoodsStateToPlayerPrefs(eType));
        });
    }
    #endregion
}
