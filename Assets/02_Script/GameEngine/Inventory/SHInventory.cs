using UnityEngine;
using System.Collections;

public enum eGoodsState
{
    NotHas  = -1,   // 가지고 있지 않음
    Disable = 0,    // 가지고 있지만 사용되지 않음
    Enable  = 1,    // 가지고 있고 사용중임
}

public partial class SHInventory : SHBaseEngine
{
    #region Virtual Functions
    public override void OnInitialize()
    {
        Clear();
        ShowCoinUI();
    }
    public override void OnFinalize() { }
    public override void OnFrameMove()
    {
        OnUpdateCoin();
        OnUpdateStick();
        OnUpdateMonster();
    }
    #endregion


    #region Interface Functions
    public void Clear()
    {
        InitCoin();
        InitStick();
        InitMonster();
    }
    #endregion


    #region Utility Functions
    #endregion
}
