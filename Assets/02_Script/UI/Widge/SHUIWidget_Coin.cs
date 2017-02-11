using UnityEngine;
using System;
using System.Collections;

public class SHUIWidget_Coin : SHMonoWrapper
{
    #region Members : Inspector
    #endregion


    #region System Functions
    #endregion


    #region Interface Functions
    #endregion


    #region Utility Functions
    #endregion


    #region Event Functions
    public override void OnCrashDamage(SHMonoWrapper pCrashObject)
    {
        var pDamage = pCrashObject as SHDamageObject;
        Single.Inventory.AddCoin((int)pDamage.m_pInfo.m_fDamageValue);
    }
    #endregion
}
