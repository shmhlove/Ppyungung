using UnityEngine;
using System;
using System.Collections;

public class SHUIPanel_HUD : SHUIBasePanel
{
    #region Members : Inspector
    [Header("HUD Widget")]
    [SerializeField] private SHUIWidget_HP   m_pHP   = null;
    [SerializeField] private SHUIWidget_Dash m_pDash = null;
    #endregion


    #region Members : Event
    #endregion


    #region System Functions
    public override void Update()
    {
        if (null != m_pHP)
            m_pHP.FrameMove();

        if (null != m_pDash)
            m_pDash.FrameMove();
    }
    #endregion


    #region Virtual Functions
    #endregion


    #region Interface Functions
    #endregion


    #region Utility Functions
    #endregion


    #region Event Handler
    #endregion
}
