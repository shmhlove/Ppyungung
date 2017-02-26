using UnityEngine;
using System;
using System.Collections;

public partial class SHPopolo : SHState
{
    bool ConnectControllerUI()
    {
        var pCtrlUI = Single.UI.GetPanel<SHUIPanel_CtrlPad>("Panel_CtrlPad");
        if (null == pCtrlUI)
            return false;

        pCtrlUI.AddEventToMove((vDirection) => m_vMoveDirection = vDirection);
        pCtrlUI.AddEventToDirection((vDirection) => m_vLookDirection = vDirection);
        pCtrlUI.AddEventToShoot(() => m_bIsShoot = true);

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