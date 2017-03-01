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

        pCtrlUI.AddEventToMove((vDirection) => m_vMoveDirection = vDirection);
        pCtrlUI.AddEventToDirection((vDirection) => m_vLookDirection = vDirection);
        pCtrlUI.AddEventToShoot(() => m_bIsShoot = true);
        pCtrlUI.AddEventToDash(() => 
        {
            if (false == m_bIsDashReady)
                return;

            m_bIsDash       = true;
            m_bIsDashReady  = false;
            SH3DRoot.SetActiveBlurCamera(true);

            SHCoroutine.Instance.WaitTime(() =>
            {
                m_bIsDash = false;
                SH3DRoot.SetActiveBlurCamera(false);
            }, SHHard.m_fCharDashTime);

            SHCoroutine.Instance.WaitTime(() =>
            {
                m_bIsDashReady = true;
            }, SHHard.m_fCharDashTime + SHHard.m_fCharDashCool);
        });
        
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