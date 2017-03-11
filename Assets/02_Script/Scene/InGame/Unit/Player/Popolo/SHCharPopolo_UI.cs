using UnityEngine;
using System;
using System.Collections;

public partial class SHCharPopolo : SHState
{
    bool ConnectControllerUI()
    {
        var pCtrlUI = Single.UI.GetPanel<SHUIPanel_CtrlPad>("Panel_CtrlPad");
        if (null == pCtrlUI)
            return false;

        pCtrlUI.Clear();
        pCtrlUI.AddEventToMove((vDirection) => m_vMoveDirection = vDirection);
        pCtrlUI.AddEventToDirection((vDirection) => m_vLookDirection = vDirection);
        pCtrlUI.AddEventToShoot(() => m_bIsShoot = true);
        pCtrlUI.AddEventToDash((bIsOn) => m_bIsDash = bIsOn);
        
        return true;
    }

    void DeConnectControllerUI()
    {
        if (false == SHUIManager.IsExists)
            return;

        var pCtrlUI = Single.UI.GetPanel<SHUIPanel_CtrlPad>("Panel_CtrlPad");
        if (null == pCtrlUI)
            return;

        pCtrlUI.Clear();
    }
}