using UnityEngine;
using System;
using System.Collections;

public class SHUIPanel_HUD : SHUIBasePanel
{
    #region Members : Inspector
    [Header("HUD Widget")]
    [SerializeField] private UILabel         m_pLabelPurpose = null;
    [SerializeField] private SHUIWidget_HP   m_pHP           = null;
    [SerializeField] private SHUIWidget_Dash m_pDash         = null;
    #endregion
    

    #region System Functions
    public override void Update()
    {
        UpdatePurpose();
        UpdateHP();
        UpdateDash();
    }
    #endregion


    #region Utility Functions
    void UpdatePurpose()
    {
        if (null == m_pLabelPurpose)
            return;

        var pPhaseInfo   = Single.GameState.GetCurrentPhaseInfo();
        var iRemainCount = pPhaseInfo.m_iPhaseCount - Single.GameState.GetCurrentKillCount();
        m_pLabelPurpose.text = Mathf.Clamp(iRemainCount, 0, iRemainCount).ToString();
    }
    void UpdateHP()
    {
        if (null == m_pHP)
            return;

        m_pHP.FrameMove();
    }
    void UpdateDash()
    {
        if (null == m_pDash)
            return;

        m_pDash.FrameMove();
    }
    #endregion
}
